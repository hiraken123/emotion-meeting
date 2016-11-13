# -*- coding: utf-8 -*-
from flask import Flask

from route import record

application = Flask(__name__)

modules_define = [
    record.app,
]
for app in modules_define:
    application.register_blueprint(app)
