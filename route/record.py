# -*- coding: utf-8 -*-
from flask import Blueprint

app = Blueprint('record', __name__, url_prefix='/')


@app.route('/record', methods=['POST'])
def record():
    return "{}"
