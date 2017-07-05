import os
import flask
import unittest
import tempfile
from Database_API import app
import json

class TestEndpoints(unittest.TestCase):
    def setUp(self):
        self.app = app.test_client()
        self.app.testing = True

    def test_getkantinedata(self):
        resp = self.app.get('/db/get/kantinedata')
        self.assertEqual(resp.status_code, 200)

    def postkantinedata(self, id, timein, timeout, date, vakantiedag, temperatuur, regen):
        return self.app.post('/db/post/kantinedata', data=dict(
            ID=id,
            timein=timein,
            timeout=timeout,
            date=date,
            vakantiedag=vakantiedag,
            temperatuur=temperatuur,
            regen=regen
        ), follow_redirects=True)

    def test_postkantinedata(self):
        data = dict("id='1' ,timein='12:33:02', timeout='12:57:02', date='2017:06:06', vakantiedag='0', temperatuur='18', regen='1'")
        #resp = self.app.post('/db/post/kantinedata', data=json.dumps(dict(id='1', timein='12:33:02', timeout='12:57:02', date='2017:06:06', vakantiedag='0', temperatuur='18', regen='1')), content_type='application/json')
        resp = self.app.post('/db/post/kantinedata', data)
        self.assertEqual(resp.status_code, 200)

        
if __name__ == '__main__':
    unittest.main()
