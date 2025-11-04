from flask import Flask, request, jsonify, make_response
from flask_mysqldb import MySQL
from flask_bcrypt import Bcrypt
import json

import time
import logging
logging.basicConfig(level=logging.DEBUG)

from flask_cors import CORS 

app = Flask(__name__)
CORS(app) 

bcrypt = Bcrypt(app)

app.config['MYSQL_HOST'] = 'localhost'
app.config['MYSQL_USER'] = ''
app.config['MYSQL_PASSWORD'] = ''
app.config['MYSQL_DB'] = ''

mysql = MySQL(app)


@app.before_request
def log_request():
    app.logger.debug(f"Request Headers: {request.headers}")
    app.logger.debug(f"Request Data: {request.get_data()}")

@app.route('/register', methods=['POST'])
def register():
    data = request.get_json()
    username = data['username']
    password = data['password']

    if not username or not password:
        return jsonify({"status": "error", "message": "Username and password are required"}), 400

    cur = mysql.connection.cursor()
    try:
        cur.execute("SELECT id FROM users WHERE username = %s", (username,))
        if cur.fetchone():
            return jsonify({"status": "error", "message": "Username already exists"}), 400

        password_hash = bcrypt.generate_password_hash(password).decode('utf-8')
        cur.execute("INSERT INTO users (username, password_hash) VALUES (%s, %s)", (username, password_hash))
        mysql.connection.commit()
        return jsonify({"status": "success", "message": "User registered successfully"}), 201
    except Exception as e:
        return jsonify({"status": "error", "message": str(e)}), 500
    finally:
        cur.close()

@app.route('/login', methods=['POST'])
def login():
    data = request.get_json()
    username = data['username']
    password = data['password']

    cur = mysql.connection.cursor()
    cur.execute("SELECT id, password_hash FROM users WHERE username = %s", (username,))
    user = cur.fetchone()
    cur.close()

    if user and bcrypt.check_password_hash(user[1], password):
        return jsonify({"status": "success", "user_id": user[0]})
    else:
        return jsonify({"status": "error", "message": "Invalid credentials"}), 401


@app.route('/save', methods=['POST'])
def save_game():
    try:
        data = request.get_json()
        data = request.get_json()
        user_id = data['user_id']
        
        
        required = ['user_id', 'scene_index', 'morality', 'diary_flags', 'play_time']
        if not all(field in data for field in required):
            return jsonify({"status": "error", "message": "Missing required fields"}), 400

        save_data = {
            "scene_index": data['scene_index'],
            "morality": data['morality'],
            "diary_flags": data['diary_flags'],
            "play_time": data['play_time']
        }

        cur = mysql.connection.cursor()
        cur.execute("""
            INSERT INTO game_saves (user_id, save_data)
            VALUES (%s, %s)
            ON DUPLICATE KEY UPDATE save_data = %s
        """, (data['user_id'], json.dumps(save_data), json.dumps(save_data)))
        
        mysql.connection.commit()
        return jsonify({
            "status": "success"
        })

    except Exception as e:
        return jsonify({"status": "error", "message": str(e)}), 500
    finally:
        if 'cur' in locals():
            cur.close()

@app.route('/load', methods=['POST'])
def load_game():
    try:
        user_id = request.get_json()['user_id']
        
        cur = mysql.connection.cursor()
        cur.execute("SELECT save_data FROM game_saves WHERE user_id = %s", (user_id,))
        result = cur.fetchone()
        
        if result:
            save_data = json.loads(result[0])
            return jsonify({
                "status": "success",
                "scene_index": save_data["scene_index"],
                "morality": save_data["morality"],
                "diary_flags": save_data["diary_flags"],
                "play_time": save_data["play_time"]
            })
        else:
            return jsonify({"status": "error", "message": "No save found"}), 404

    except Exception as e:
        return jsonify({"status": "error", "message": str(e)}), 500
    finally:
        if 'cur' in locals():
            cur.close()


@app.route('/delete_save', methods=['POST'])
def delete_save():
    try:
        data = request.get_json()
        user_id = data['user_id']
        
        cur = mysql.connection.cursor()
        cur.execute("DELETE FROM game_saves WHERE user_id = %s", (user_id,))
        mysql.connection.commit()
        
        if cur.rowcount > 0:
            return jsonify({"status": "success", "message": "Save deleted successfully"})
        else:
            return jsonify({"status": "error", "message": "No save found to delete"}), 404

    except Exception as e:
        return jsonify({"status": "error", "message": str(e)}), 500
    finally:
        if 'cur' in locals():
            cur.close()

@app.route('/save_info', methods=['POST'])
def save_info():
    try:
        user_id = request.get_json()['user_id']
        
        cur = mysql.connection.cursor()
        cur.execute("SELECT save_data FROM game_saves WHERE user_id = %s", (user_id,))
        result = cur.fetchone()
        
        if result:
            save_data = json.loads(result[0])
            return jsonify({
                "info": {
                    "sceneIndex": save_data["scene_index"]
                }
            })
        else:
            return jsonify({"info": None})

    except Exception as e:
        return jsonify({"status": "error", "message": str(e)}), 500
    finally:
        if 'cur' in locals():
            cur.close()

@app.route('/check_save', methods=['POST'])
def check_save():
    try:
        user_id = request.get_json()['user_id']
        
        cur = mysql.connection.cursor()
        cur.execute("SELECT 1 FROM game_saves WHERE user_id = %s", (user_id,))
        result = cur.fetchone()
        
        return jsonify({"exists": result is not None})

    except Exception as e:
        return jsonify({"status": "error", "message": str(e)}), 500
    finally:
        if 'cur' in locals():
            cur.close()






if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, debug=True)