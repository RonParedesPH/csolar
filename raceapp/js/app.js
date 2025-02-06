class App {
    constructor() {
        this._init()
    }

_init() {
    window["Params"] = {AppName:"Race App Sandbox"};
    window.Params.LoginPage = "pages.authentication.login.html";
    window.Params.LandingPage = "pages.landing.html";

}

}

const a = new App();

export default App
