//var User;
// var Messages;
// var Toasts;
// var Routes;

var geta;


class UserDomInject {
    constructor() {

    }

    //public
    init() {
            if ( window.Params !== undefined)
            {
                document.title = window.Params.AppName;
            }

             /* hide common elements
            */
            let itemsToHide = [
                '[href="#apps"]',
                '[href="#pages"]',
                '[href="#blocks"]',
                '[href="#interface"]'
            ];
            itemsToHide.forEach( item => {
                let it = document.querySelector(item);
                if (it?1:0)
                    it.closest('li').classList.add('d-none');
            });

            let hideIt = document.querySelector('.language-button');
            if (hideIt ? 1 : 0)
                hideIt.classList.add('d-none');
            hideIt = document.querySelector('#settingsButton');
            if (hideIt ? 1 : 0)
                hideIt.classList.add('d-none');
            hideIt = document.querySelector('#nichesButton');
            if (hideIt ? 1 : 0)
                hideIt.classList.add('d-none');

            hideIt = document.querySelector(".profile");
            if (hideIt ? 1 : 0)
                hideIt.classList.add('d-none');

            /* alter common elements
            */
            let alterIt = document.querySelector(".logo > a");
            if (alterIt ? 1 : 0)
                alterIt.setAttribute("href", "index.html");
        alterIt = document.querySelector('.user>.name');
        if (alterIt ? 1 : 0)
            alterIt.innerHTML = "";

        //const user = new User()
        //user.readFromStorage();
        //if (user.token ? 1 : 0) {

        // use the instance attached to window object: see user.js
        
        if ((window.currentUser ? 1 : 0) && (window.currentUser.token ? 1 : 0)) {
            const user = window.currentUser;
            /* install handlers 
            */

            const profileElement = document.getElementsByClassName("profile");
            if (profileElement.length > 0) {

                const nameElement = document.querySelector('.user>.name');
                nameElement.innerHTML = user.fullName();
                if (user.profile.profilepic ? 1 : 0) {
                    const profilePic = document.querySelector('.user>img.profile');
                    profilePic.setAttribute('src', `img/profile/${user.profile.profilepic}`)
                    profilePic.classList.remove('d-none');
                }
                else 
                 profilePic.classList.add('d-none');
            }   

            //ToDO: document.querySelector('.align-middle[data-bs-target="#settings"]')
            let them = document.querySelectorAll(".align-middle");
            them.forEach((el) => {
                if (el.innerText === "Logout" && document.querySelectorAll(".logout").length == 0) {
                    el.addEventListener("click", (event) => {
                        const messages = new Messages();
                        const toast = new Toasts()
                        toast.Toast(messages.AuthAreYouSureToLogout, "bg-transparent", true, () => {
                            const toast = new Toasts()
                            toast.Toast(messages.AuthUserLogout, "bg-warning");
                            setTimeout(() => {
                                user.removeFromStorage();
                                window.location = "Pages.Authentication.Login.html";
                            }, 1000);
                        });
                    });
                }
                if (el.innerText === "Settings") {
                    el.addEventListener("click", (event) => {
                        console.log("user settings pressed");
                    });
                }
            });
            them = document.querySelectorAll(".logout");
            them.forEach((el) => {
                // if (el.innerText === "Logout") {
                el.addEventListener("click", (event) => {
                    event.preventDefault;
                    event.stopPropagation();
                    const messages = new Messages()
                    const toast = new Toasts()

                    toast.Toast(messages.AuthAreYouSureToLogout, "bg-transparent", true, () => {
                        toast.Toast(messages.AuthUserLogout, "bg-warning");
                        setTimeout(() => {
                            user.removeFromStorage();
                            window.location = "pages.authentication.login.html";
                        }, 1000);
                    });
                });
                // }
                // if (el.innerText === "Settings") {
                //     el.addEventListener("click", (event) => {
                //         console.log("user settings pressed");
                //     });
                // }
            });

            them = document.querySelectorAll("a");
            them.forEach((el) => {
                if (el.innerText === "User Info") {
                    //el.addEventListener("click", (event) => {
                    //    console.log("user info pressed");
                    //});
                    let temp = "";
                    user.rolesList.forEach((r) => temp += `<li>${r.Name}</li>`);

                    let p = el.parentElement.parentElement;     //obtain UL
                    p.innerHTML = temp;
                }
            });

            if (window.location.toString().toLowerCase().indexOf('pages.landing') > 0) {
                them = document.querySelectorAll(".card .h-100");
                if (them.length) {
                    them[0].innerHTML =
                        "Welcome " +
                        user.nickName().bold() +
                        ".<br/>" +
                        (user.rolesList.length
                            ? "The system recognizes you to have been granted the role(s) of : " +
                            user.roles().bold()
                            : "Your account has not been granted yet with any role, please contact support.");
                }
            }

            them = document.querySelector("#menu");
            if (them ? 1 : 0) {
                let i = 0;
                let headr = '';
                let templat = '';
                let items = [];        
 

                user.routeList.forEach((r) => {
                    let item = `item__${i}`
                    if (headr !== r.Name.split('/')[0]) {
                        if (headr.length) {
                            items.push(templat.replace('^', ''));
                            templat = '';
                        }
                        headr = r.Name.split('/')[0]
                        templat = `<a href="#${item}" data-href="${item}.html">
                            <i data-acorn-icon="web-page" class="icon" data-acorn-size="18"></i>
                            <span class="label">${headr}</span>
                            </a>
                            <ul id="${item}">
                                ^--
                            </ul>
                        `;
                    }
                    let titl = r.Name.split('/')[1];
                    let tmp = templat.split('^--')[0] +
                        `<li>
                            <a href="${r.Path}" class="route" data-route="${r.Name}">
                                <span class="label">${titl}</span>
                            </a>
                            </li> ^--` +
                        templat.split('^--')[1];
                    templat = tmp;

                    i++;
                });
                if (templat.length)
                    items.push(templat.replace('^--', ''));

                if (items.length) {
                    // let kite = '';

                    items.forEach((t) => {
                        // // let e = document.createElement('li');
                        // // //e.classList.add("dropdown");
                        // // e.innerHTML = t.replace('^--', '').trim();
                        // // them.appendChild(e);
                        // kite += `<li>${t}</li>`;

                        let e = document.createElement('li');
                        e.innerHTML = t;
                        them.appendChild(e);

                    });

                    // // create an executable code block than will be called at the proper
                    // // timing based on wether the menuPlainInner property of Nav is already set
                    // const block = function(delay) {
                    //     //console.log('delay at ' + delay)
                    //     if (window.nav.menuPlainInner !== undefined) {
                    //         const a = window.nav.menuPlainOuter.split(window.nav.menuPlainInner);
                    //         window.nav.menuPlainOuter = a[0] + window.nav.menuPlainInner + kite + a[1];
                    //         window.nav.RedrawMenuPlacement();                  
                    //     } 
                    //     //else console.log('called while undefined.' )
                    // }

                    // if (window.nav.menuPlainInner !== undefined) {
                    //     block(0);
                    // }
                    // else 
                    //     setTimeout( 
                    //         () =>  { block(200) }, 
                    //         200);  // <-- call delay at 200ms
                }
                 //console.log('user menu at userDomInject - Ok')
            }
        }
        // else
        //     window.location = "pages.authentication.login.html";         //<---- activate this on production

        document.body.classList.add('acornPageReady');
    }

    // //public
    // setEventHandlers() {
    //     const routeNodes = document.querySelectorAll('.route');

    //     const routes = new Routes()
    //     routeNodes.forEach(r => {
    //         let path = routes.getTarget(r.dataset.route)
    //         if (path ? 1 : 0)
    //             r.setAttribute('href',path)
    //     });
    // }
}


mudPool.depends([
    './user.js',
    './helpers/messages.js',
    './helpers/toasts.js',
], (user, messages, toasts) => {
    // Messages = messages;
    // Toasts = toasts;

    const u = new UserDomInject();
    u.init();
    // u.setEventHandlers();
});

export default UserDomInject;
