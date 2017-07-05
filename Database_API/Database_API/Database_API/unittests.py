import os
import unittest
import flask
from Database_API import app
from Database_API.views import *


class TestDatabaseEndpoints(unittest.TestCase):
    def setUp(self):
        self.app = app.test_client()
        self.app.testing = True

    def test_getkantinedata(self):
        resp = self.app.get('/db/get/kantinedata')
        self.assertEqual(resp, json)

    def test_postdata():
        data = {"id":"2", "timein":"19:20:36", "timeout":"19:40:42", "date":"2017:06:24","vakantiedag":"0","temperatuur":"21","regen":"0"}
        data = jsonify(data)
        resp = self.app.post('/db/post/kantinedata',data)
        self.assertEqual(resp.status_code, 200)

test = TestDatabaseEndpoints()
test.testgetkantinedata()

