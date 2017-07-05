import os
import unittest
import flask
from Database_API import app
from Database_API.views import *


class TestDatabaseEndpoints(unittest.TestCase):
    def setUp(self):
        self.app = app.test_client()
        self.app.testing = True

    def testgetkantinedata(self):
        resp = self.app.get('/db/get/kantinedata')
        self.assertEqual(resp, json)

test = TestDatabaseEndpoints()
test.testgetkantinedata()

