var app = new Vue({
    el: '#app',
    data: {
        cId: null,
        cInfo: null,
        linkToPortal: null,
        post: {
            cId: null,
            comContent: null,
            comTime: null,
            mId: null
        },
        comments: null,
    },
    watch: {

    },
    created: function () {
        this.cId = document.getElementById("course").innerHTML;
        this.post.mId = document.getElementById("mId").innerHTML;
        console.log(this.cId);
        console.log(this.post.mId);
        this.getCourse();
        this.getComments();
        //this.linkToPortal = "https://portal.yzu.edu.tw/cosSelect/Cos_Plan.aspx?y=105&s=2&id=" + this.cInfo[0].cId.slice(0, 4) + "&c=" + this.cInfo[0].cId.slice(6, 6);
    },
    methods: {
        addComment: function () {
            let self = this;
            if (!this.post.comContent) {
                alert("請輸入留言!!!");
                return;
            }
            // Insert Metadata
            this.post.comTime = this.getTimeNow();
            this.post.cId = this.cId;
            // Convert to JSON
            var jPost = JSON.stringify(self.post);
            fetch("/api/Comments", {
                headers: {
                    'Content-Type': 'application/json'
                },
                method: "POST",
                body: jPost
            })
                .then(function (res) {
                    if (!res.created || !res.ok) {
                        console.log(res.statusText);
                    }
                    self.post.comContent = null;
                    self.getComments();
                })
        },
        getComments: function () {
            let self = this;
            var connString = "/api/Comments?$select=mId,Member/mNickname,comSeq,comContent,comTime&$expand=Member&$filter=cId eq '" + self.cId + "'";
            console.log("getCom" + connString);
            fetch(connString)
                .then(function (res) {
                    if (!res.ok) throw new Error(res.statusText);
                    return res.json();
                })
                .then(function (data) {
                    if (data.value.length == 0) {
                        self.comments = null;
                        return;
                    }
                    self.comments = data.value;
                    console.log(self.comments);
                })
        },
        getCourse: function () {
            let self = this;
            var connString = "/api/Courses?$expand=Teacher,CourseRoom&$filter=cId eq '" + self.cId + "'";
            console.log("getCou" + connString);
            fetch(connString)
                .then(function (res) {
                    if (!res.ok) throw new Error(res.statusText);
                    return res.json();
                })
                .then(function (data) {
                    if (data.value.length == 0) {
                        return;
                    }

                    self.cInfo = data.value;
                    self.linkToPortal = "https://portal.yzu.edu.tw/cosSelect/Cos_Plan.aspx?y=105&s=2&id=" + data.value[0].cId.slice(0, 5) + "&c=" + data.value[0].cId.slice(6, 8);
                    console.log(self.cInfo);
                })
        },
        delComment: function (id) {
            let self = this;
            var connString = "/api/Comments(" + id + ")";
            fetch(connString, {
                method: "DELETE"
            })
                .then(function (res) {
                    console.log(res.statusText);
                    self.getComments();
                })
        },
        getTimeNow: function () {
            return new Date().toISOString().slice(0, 19).replace('Z', ' ');
        },
        getImgUrl: function (id) {
            var avatarNo = id % 7;
            return "/Content/avatar" + avatarNo + ".png";
        }

    }
});