/**
 *
 * AuthLogin
 *
 * Pages.Authentication.Login page content scripts. Initialized from scripts.js file.
 *
 *
 */


class AuthLoginForm {
    constructor() {

    }

    //public this
    init() {
        const form = document.getElementById("loginForm");
        if (!form) {
            return;
        }

        const validateOptions = {
            rules: {
                email: {
                    required: true,
                    email: true,
                },
                password: {
                    required: true,
                },
            },
            messages: {
                email: {
                    email: "Your email address must be in correct format!",
                },
            }, 
        };

        mudPool.depends([
            './api/auth.api.js',
            './helpers/routes.js'
        ], (authApi, routes) => {
            
            jQuery(form).validate(validateOptions);
            form.addEventListener("submit", (event) => {
                event.preventDefault();
                event.stopPropagation();
                if (jQuery(form).valid()) {
                    const formValues = {
                        Email: form.querySelector('[name="email"]').value,
                        Password: form.querySelector('[name="password"]').value,
                    };
                    document.querySelector('button[type="submit"]').disabled = true;
                    document.querySelector(".spinner-border").style.display = "block";
                    const authApi = new AuthApi()
                    authApi.Request_Login(
                        formValues,
                        (t) => {
                                const resp = JSON.parse(t);
                                const user = new User();

                                user.profile.username = resp.UserName;
                                user.profile.firstname = resp.UserName;
                                user.profile.lastname = ' ';
                                user.profile.profilepic = `profile-11.webp`;
                                user.token = resp.LoginSession.Id;
                                user.rolesList = resp.Roles;

                                let routes = [];
                                const r = new Routes();
                                resp.Routes.forEach( e => {
                                    routes.push({Name: e.Name, PropsList: e.PropsList, Path: r.getTarget(e.Name)})
                                });

                                user.routeList = routes;
                                user.lastStamp = new Date().toISOString();
                                user.writeToStorage();

                                // resolve if can resume to last page
                                let resumeToPage = sessionStorage.getItem("ResumeToPage");
                                if (resumeToPage!==null && 
                                    routes.find( o => {
                                        if (o.Path===resumeToPage) return true
                                    })  === undefined) {
                                    resumeToPage = window.Params !== undefined ? window.Params.LandingPage : "";
                                }

                                sessionStorage.removeItem("ResumeToPage");
                                window.location =  resumeToPage === null || resumeToPage.length === 0? "pages.landing.html" : resumeToPage;
                        },
                        (h) => {
                            console.log(h);
                            form.querySelector('[name="password"]').value = '';
                            
                            //let msg = 'WARNING: API backend is unreachable';
                            const messages = new Messages()
                            let msg =  messages.AuthenticationFatalError;
                            if (h.responseText? true: false)
                            msg = JSON.parse(h.responseText).Message;
                            else if (h.responseText)
                                msg = h.responseText;
                            document.querySelector('button[type="submit"]').disabled = false;
                            document.querySelector(".spinner-border").style.display = "none";

                            const toast = new Toasts();
                            toast.Toast(msg, "bg-danger");
                        },
                        () => {
                            document.querySelectorAll(
                                'button[type="submit"]'
                            )[0].disabled = false;
                        }                    
                    );
                }
            });
        });

    }
}


mudPool.depends([
    './app.js',
    './xtempl.js',
    './user.js',
    './helpers/messages.js',
    './helpers/toasts.js'
], (app, xtempl, user, messages, toasts) => {
        const c = new xtempl();
        c.relocate();

        mudPool.depends([
          "./base/_helpers.js",
          "./base/_settings.js",
           "./base/_nav.js",
          "./_common.js",
          "./_scripts.js",
        ],
        (helpers, settings, nav, common, scripts) => {
          const s = new scripts();

        const r = new AuthLoginForm()
        r.init();
    });
})


// don't export as this is not expected to be called anywhere
//export default Auth;
