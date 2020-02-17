async function getData(url) {
    const response = await fetch(url, {
        method: "GET",
        headers: {
            "Accept": "application/json"
        }
    });

    if (response.ok === true) {
        return await response.json();
    } else {
        return console.log("Status: ", response.status);
    }
};

async function getMainPageAuthFragment() {
    let user = "";

    try {
        user = await getData("/api/values/getlogin");
    } catch (e) {
        document.getElementById("login-status-field").innerHTML = 
            `<a href="/User/LogIn">Log In</a>
            |
             <a href="/User/SignIn">Sign In</a>`;
    }

    if (user == null || user == "" || user == undefined) {
        document.getElementById("login-status-field").innerHTML = 
            `<a href="/User/LogIn">Log In</a>
            |
             <a href="/User/SignIn">Sign In</a>`;
    } else {
        document.getElementById("login-status-field").innerHTML = 
            `<a href="/User/LogOut">Log Out</a>
             <p>Current user: ${user} </p>`;
    }
}

async function changeInputValue(inputId) {
    let inputValue = await getData("/api/values/getlogin");
    document.getElementById(inputId).value = inputValue;
}