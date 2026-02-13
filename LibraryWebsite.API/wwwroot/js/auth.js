function login() {
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;
    const error = document.getElementById("error");

    error.innerText = "";

    if (!username || !password) {
        error.innerText = "Username and password are required";
        return;
    }

    fetch("https://localhost:7290/api/User/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            username: username,
            password: password
        })
    })
        .then(res => {
            if (!res.ok)
                throw new Error("Invalid username or password");
            return res.json();
        })
        .then(data => {
            localStorage.setItem("token", data.token);

            // decode role from token (simple way)
            const payload = JSON.parse(atob(data.token.split('.')[1]));
            const role = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

            if (role === "Admin") {
                window.location.href = "admin.html";
            } else {
                window.location.href = "index.html";
            }
        })
        .catch(err => {
            error.innerText = err.message;
        });
}
