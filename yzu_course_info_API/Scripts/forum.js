var app = new Vue({
    el: "#app",
    data: {
        courses: null,
        teachers: null,
        numsOfRows: "0",
        cIdKey: null,
        cNameKey: null,
        tNameKey: null,
        connString: null,
        filter: true,
    },
    created: function () {
    },
    watch: {
        cIdKey: function () {
            this.findCourses();
        },
        cNameKey: function () {
            this.findCourses();
        },
        tNameKey: function () {
            this.findCourses();
        },
        numsOfRows: function () {
            this.findCourses();
        },
    },
    methods: {
        order: function (col) {
            var order = this.connString + '&$orderby=' + col;
            if (this.filter)
                this.filter = false;
            else {
                this.filter = true;
                order += ' desc';
            }
            console.log(order);
            this.getCourses(order);
        },
        findCourses: function () {
            let self = this;
            if (!self.numsOfRows)
                self.numsOfRows = "0";
            var connString = '/api/Courses?$expand=Teacher&$top=' + self.numsOfRows;
            if (self.cIdKey || self.cNameKey || self.tNameKey) {
                connString += "&$filter=";
                if (self.cIdKey) {
                    connString += "substringof('" + self.cIdKey + "', cId)";
                    if (self.cNameKey || self.tNameKey)
                        connString += " and ";
                }
                if (self.cNameKey) {
                    connString += "substringof('" + self.cNameKey + "', cName)";
                    if (self.tNameKey)
                        connString += " and ";
                }
                if (self.tNameKey)
                    connString += "substringof('" + self.tNameKey + "', Teacher/tName)";
            }
            self.connString = connString;
            console.log(connString);
            self.getCourses(connString);
            //console.log(connString);
        },
        getCourses: function (connString) {
            let self = this;
            fetch(connString)
                .then(function (res) {
                    if (!res.ok) throw new Error(res.statusText);
                    return res.json();
                })
                .then(function (data) {
                    if (data.value.length == 0) {
                        self.courses = null;
                        return;
                    }
                    self.courses = data.value;
                    console.log(self.courses);
                })

        },
    }
})