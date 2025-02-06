class User {
    constructor() {
        // Initialization of the page plugins
        this.token = null;
        this.profile = { firstname: "", lastname: "", profilepic: "", username: "" };
        this.rolesList = [];
        this.routeList = [];
        this.lastStamp = new Date("01/01/00").toISOString();
        this._init();
    }

    readFromStorage = function () {
        let data = sessionStorage.getItem("acorn-user");
        if (data) {
            let obj = JSON.parse(data);
            if (obj) {
                this.lastStamp = obj.lastStamp;
                this.profile = obj.profile;
                this.token = obj.token;
                this.rolesList = obj.rolesList;
                this.routeList = obj.routeList;
                this.routeList.sort((a, b) => {
                    return a.Name == b.Name ? 0 :
                        a.Name > b.Name ? 1 : -1;
                })
            }
        }

        return this.dataset();
    }

    dataset = function() {
        return {
            lastStamp: this.lastStamp,
            token: this.token,
            profile: this.profile,
            rolesList: this.rolesList,
            routeList: this.routeList,
        }
    }

    writeToStorage = function () {
        let obj = this.dataset();
        sessionStorage.setItem("acorn-user", JSON.stringify(obj));
    }

    removeFromStorage = function () {
        sessionStorage.removeItem("acorn-user");
    }

    fullName = function() {
        return this.profile.firstname;
        // return this.profile.firstname[0].toUpperCase()+
        //         this.profile.firstname.substring(1)+
        //         ' ' +
        //         this.profile.lastname[0].toUpperCase()+
        //         this.profile.lastname.substring(1);
    }

    nickName = function() {
       return this.profile.firstname;
        // return this.profile.firstname[0].toUpperCase() + 
        //         this.profile.firstname.substring(1);
        //return 'nickName'
    }

    roles = function() {
        let tmp = ''
        this.rolesList.forEach( (r) => (tmp += (tmp !== "" ? ", " : "") + r.Name));
        return tmp;
    }

    routes  = function() {

        let tmp = []
        this.routeList.forEach( (r) => (tmp.push({name:r.Name, rights: r.PropsList})));
        tmp.sort((a,b)=>{
            return a.Name == b.Name ? 0 : 
                a.Name > b.Name ? 1 : -1;
        })        
        return tmp;
    }


    _init() {
        this.readFromStorage();
    }

}

let loginPage = 'pages.authentication.login.html';
if (window.Params !== undefined) {
    document.title = window.Params.AppName;
    loginPage = window.Params.LoginPage;
}

window.currentUser = new User();
const currentLocation = document.location.toString().split('/').pop();
if (currentLocation.toLowerCase().indexOf(".auth") === -1) {
    if (window.currentUser.token !== null) { 
        if (Date.parse( new Date().toISOString()) - Date.parse(window.currentUser.lastStamp) > 30  *60*1000) {
            
            sessionStorage.setItem("ResumeToPage", currentLocation)
            window.currentUser.removeFromStorage();
            window.location = loginPage;
        }
        // if (currentLocation.toLocaleLowerCase().indexOf("pages.landing")===-1 && 
        //     window.currentUser.routeList.find(o=>{ if (o.Path === currentLocation ) return true }) ===undefined) 
        // {
        //     window.currentUser.removeFromStorage();
        //     window.location = loginPage;
        // }
    }
    else {
        window.currentUser.removeFromStorage();
        window.location = loginPage;
    } 
}

export default User;
