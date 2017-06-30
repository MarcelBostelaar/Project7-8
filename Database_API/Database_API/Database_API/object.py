

class Object:
    _id = None
    _time_in = None
    _time_out = None
    _date = None
    _rain = None
    _temperature = None
    _vacation = None

    def __init__(self, id, time_in, time_out, date, vacation, temperature, rain):
        self._id = id
        self._time_in = time_in
        self._time_out = time_out
        self._date = date
        self._vacation = vacation
        self._temperature = temperature
        self._rain = rain



    #
    #PROPERTIES
    #

    @property
    def id(self):
        return self._id

    @id.setter
    def id(self, value):
        self._id = value

    @property
    def time_in(self):
        return self._time_in

    @time_in.setter
    def time_in(self, value):
        self._time_in = value

    @property
    def time_out(self):
        return self._time_out

    @time_out.setter
    def time_out(self, value):
        self._time_out = value

    @property
    def date(self):
        return self._date

    @date.setter
    def date(self, value):
        self._date = value

    @property
    def vacation(self):
        return self._vacation

    @vacation.setter
    def vacation(self, value):
        self._vacation = value

    @property
    def temperature(self):
        return self._temperature

    @temperature.setter
    def temperature(self, value):
        self._temperature = value

    @property
    def rain(self):
        return self._rain

    @rain.setter
    def rain(self, value):
        self._rain = value

    def serialize(self):
        return {
            "id": self._id,
            "time_in": self._time_in,
            "time_out": self._time_out,
            "date": self._date,
            "vacation": self._vacation,
            "temperature": self._temperature,
            "rain": self._rain
        }
