
//var User;
//var Messages;
//var Toasts;
//var UserDomInject;


class LandingPage {
    constructor() {
    }
}

mudPool.depends(
    [
          "./app.js",
          "./user.js",
          "./user.DomInject.js",
        ],
        (app, user, domInject) => {
 
      mudPool.depends(
        [
         "./base/_helpers.js",
         "./base/_settings.js",
          "./base/_nav.js",
          "./_common.js",
          "./_scripts.js",
        ],
        (helpers, settings,  nav, common, scripts) => {
          const s = new scripts();
          const r = new LandingPage();
        }
    )
}
)
