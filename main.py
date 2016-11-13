#!/usr/bin/env python
# -*- coding: utf-8 -*-

from route import application

if __name__ == "__main__":
    print "Application Start"

    application.run('0.0.0.0', debug=True, port=8080)
