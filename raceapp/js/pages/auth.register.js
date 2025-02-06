/**
 *
 * AuthRegisterForm
 *
 * Pages.Authentication.Register page content scripts. Initialized from scripts.js file.
 *
 *
 */


class AuthRegisterForm {
    constructor() {

    }

    // Public
    init() {
       const form = document.getElementById("registerForm");
        if (!form) {
            return;
        }
        const validateOptions = {
            rules: {
                registerEmail: {
                    required: true,
                    email: true,
                },
                registerPassword: {
                    required: true,
                    minlength: 6,
                    regex: /[a-z].*[0-9]|[0-9].*[a-z]/i,
                },
                registerCheck: {
                    required: true,
                },
                registerName: {
                    required: true,
                },
            },
            messages: {
                registerEmail: {
                    email: "Your email address must be in correct format!",
                },
                registerPassword: {
                    minlength: "Password must be at least {0} characters!",
                    regex: "Password must contain a letter and a number!",
                },
                registerCheck: {
                    required: "Please read and accept the terms!",
                },
                registerName: {
                    required: "Please enter your name!",
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
                    email: form.querySelector('[name="registerEmail"]').value,
                    password: form.querySelector('[name="registerPassword"]').value,
                    username: form.querySelector('[name="registerName"]').value,
                    check: form.querySelector('[name="registerCheck"]').checked,
                };
                console.log(formValues);
                //return;
                //let api = new AuthApi();
                document.querySelectorAll('button[type="submit"]')[0].disabled = true;
                //const authApi = new AuthApi()
                this._authApi.Request_Register(
                    formValues,
                    (t) => {
                        console.log(t);

                        //const messages = new Messages()
                        let msg = this._messages.AuthRegistrationSuccess;

                        const toasts = new this._Toasts()
                        toasts.Toast(msg, "bg-primary");

                        setTimeout(() => {
                            window.location = "pages.authentication.login.html";
                        }, 2000);
                    },
                    (h) => {
                        console.log(h);

                        //let msg = 'WARNING: API backend is unreachable';
                        //const messages = new Messages()
                        let msg = this._messages.AuthenticationFatalError;
                        if (h.responseText ? true : false)
                            msg = JSON.parse(h.responseText).Message;
                        else if (h.responseText)
                            msg = h.responseText;

                        //let toast = new ComponentsToasts();
                        const toasts = new this._Toasts()
                        toasts.Toast(msg, "bg-danger");
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
        
        const r = new AuthRegisterForm()
        r.init() 
    });
})

