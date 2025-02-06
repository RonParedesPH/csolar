/**
 *
 * Loader
 * Adds a spinner class to the body if the DOMContentLoaded is not fired for 500 ms.
 * This prevents seeing a spinner on an already visited page.
 *
 **/

(function () {
  let isContentLoaded = false;
 
  document.body.classList.add('spinner');
 
  // const timeoutId = setTimeout(() => {
  //   if (!isContentLoaded) {
  //     document.body.classList.add('spinner');
  //   }
  // }, 500);

  window.addEventListener('DOMContentLoaded', (event) => {
    isContentLoaded = true;
  });
})();
