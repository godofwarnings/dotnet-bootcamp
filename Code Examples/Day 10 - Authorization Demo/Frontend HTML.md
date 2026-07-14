# Frontend HTML (Day 10)

This file contains the client-side HTML interfaces for the Authorization Demo application.

### `wwwroot/index.html`
```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>CHUBB Trainings</title>
    <meta http-equiv="refresh" content="0; url=/login.html" />
</head>
<body>
    Redirecting to login...
</body>
</html>
```

### `wwwroot/login.html`
```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>CHUBB Trainings Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        body {
            margin: 0;
            font-family: Arial, Helvetica, sans-serif;
            background: #f4f7fb;
            display: flex;
            min-height: 100vh;
            align-items: center;
            justify-content: center;
        }

        .card {
            width: 380px;
            background: white;
            border-radius: 12px;
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.08);
            padding: 24px;
        }

        h1 {
            margin-top: 0;
            font-size: 24px;
            color: #1d3557;
        }

        label {
            display: block;
            margin-top: 12px;
            margin-bottom: 6px;
            font-weight: bold;
        }

        input {
            width: 100%;
            box-sizing: border-box;
            padding: 10px;
            border: 1px solid #cfd8e3;
            border-radius: 8px;
        }

        button {
            width: 100%;
            margin-top: 16px;
            padding: 12px;
            border: none;
            border-radius: 8px;
            background: #1d4ed8;
            color: white;
            font-size: 16px;
            cursor: pointer;
        }

        .error {
            color: #b00020;
            margin-top: 12px;
            min-height: 20px;
        }

        .demo {
            margin-top: 16px;
            background: #eef4ff;
            border-radius: 8px;
            padding: 12px;
            font-size: 14px;
        }

            .demo code {
                background: #dce8ff;
                padding: 2px 6px;
                border-radius: 4px;
            }
    </style>
</head>
<body>
    <div class="card">
        <h1>CHUBB Trainings Login</h1>

        <form id="loginForm">
            <label for="username">UserName</label>
            <input id="username" type="text" autocomplete="username" required />

            <label for="password">Password</label>
            <input id="password" type="password" autocomplete="current-password" required />

            <button type="submit">Login</button>
        </form>

        <div id="errorMessage" class="error"></div>

        <div class="demo">
            <div><strong>Demo credentials</strong></div>
            <div>Admin: <code>admin</code> / <code>admin123</code></div>
            <div>Trainer: <code>trainer</code> / <code>trainer123</code></div>
        </div>
    </div>

    <script type="module" src="./js/login.js"></script>
</body>
</html>
```

### `wwwroot/admin.html`
```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>Admin Dashboard</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        body {
            margin: 0;
            font-family: Arial, Helvetica, sans-serif;
            background: #f5f7fb;
        }

        .topbar {
            background: #1d3557;
            color: white;
            padding: 16px 24px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .container {
            max-width: 1100px;
            margin: 24px auto;
            padding: 0 16px;
        }

        .grid {
            display: grid;
            gap: 16px;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
        }

        .card {
            background: white;
            border-radius: 12px;
            box-shadow: 0 8px 20px rgba(0, 0, 0, 0.06);
            padding: 20px;
        }

        h2, h3 {
            margin-top: 0;
        }

        input, button {
            padding: 10px;
            border-radius: 8px;
            border: 1px solid #ccd6e0;
        }

        input {
            width: 100%;
            box-sizing: border-box;
            margin-bottom: 10px;
        }

        button {
            background: #1d4ed8;
            color: white;
            border: none;
            cursor: pointer;
        }

        .danger {
            background: #c62828;
        }

        ul {
            padding-left: 18px;
        }

        li {
            margin-bottom: 10px;
        }

        .row {
            display: flex;
            gap: 8px;
            align-items: center;
            justify-content: space-between;
        }

        .muted {
            color: #555;
            font-size: 14px;
        }

        .stats {
            display: flex;
            gap: 16px;
            flex-wrap: wrap;
        }

        .pill {
            background: #eef4ff;
            padding: 10px 14px;
            border-radius: 999px;
        }

        .error {
            color: #b00020;
            margin-top: 8px;
        }
    </style>
</head>
<body>
    <div class="topbar">
        <div>
            <strong>Admin Dashboard</strong>
            <div id="welcomeText" class="muted"></div>
        </div>
        <button id="logoutButton" type="button">Logout</button>
    </div>

    <div class="container">
        <div class="card">
            <h2>Overview</h2>
            <div class="stats">
                <div class="pill">Cohorts: <span id="cohortCount">0</span></div>
                <div class="pill">Trainers: <span id="trainerCount">0</span></div>
            </div>
            <div id="dashboardMessage" class="muted" style="margin-top:12px;"></div>
        </div>

        <div class="grid" style="margin-top:16px;">
            <div class="card">
                <h3>Manage Cohorts</h3>
                <form id="cohortForm">
                    <input id="cohortName" type="text" placeholder="Cohort name" required />
                    <input id="cohortStartDate" type="date" required />
                    <button type="submit">Add Cohort</button>
                </form>
                <div id="cohortError" class="error"></div>
                <ul id="cohortList"></ul>
            </div>

            <div class="card">
                <h3>Manage Trainers</h3>
                <form id="trainerForm">
                    <input id="trainerUsername" type="text" placeholder="Username" required />
                    <input id="trainerPassword" type="password" placeholder="Password" required />
                    <input id="trainerFullName" type="text" placeholder="Full name" required />
                    <input id="trainerSpecialty" type="text" placeholder="Specialty" />
                    <button type="submit">Add Trainer</button>
                </form>
                <div id="trainerError" class="error"></div>
                <ul id="trainerList"></ul>
            </div>
        </div>
    </div>

    <script type="module" src="./js/admin.js"></script>
</body>
</html>
```

### `wwwroot/trainer.html`
```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>Trainer Dashboard</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        body {
            margin: 0;
            font-family: Arial, Helvetica, sans-serif;
            background: #f5f7fb;
        }

        .topbar {
            background: #1f6f50;
            color: white;
            padding: 16px 24px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .container {
            max-width: 1100px;
            margin: 24px auto;
            padding: 0 16px;
        }

        .grid {
            display: grid;
            gap: 16px;
            grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
        }

        .card {
            background: white;
            border-radius: 12px;
            box-shadow: 0 8px 20px rgba(0, 0, 0, 0.06);
            padding: 20px;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        th, td {
            text-align: left;
            padding: 10px;
            border-bottom: 1px solid #e2e8f0;
        }

        h2, h3 {
            margin-top: 0;
        }

        button {
            padding: 10px 14px;
            border-radius: 8px;
            border: none;
            background: #1d4ed8;
            color: white;
            cursor: pointer;
        }

        .muted {
            color: #555;
            font-size: 14px;
        }

        .stats {
            display: flex;
            gap: 16px;
            flex-wrap: wrap;
        }

        .pill {
            background: #eefcf5;
            padding: 10px 14px;
            border-radius: 999px;
        }
    </style>
</head>
<body>
    <div class="topbar">
        <div>
            <strong>Trainer Dashboard</strong>
            <div id="welcomeText" class="muted"></div>
        </div>
        <button id="logoutButton" type="button">Logout</button>
    </div>

    <div class="container">
        <div class="card">
            <h2>Overview</h2>
            <div class="stats">
                <div class="pill">Upcoming Sessions: <span id="sessionCount">0</span></div>
                <div class="pill">Attendance Records: <span id="attendanceCount">0</span></div>
            </div>
            <div id="dashboardMessage" class="muted" style="margin-top:12px;"></div>
        </div>

        <div class="grid" style="margin-top:16px;">
            <div class="card">
                <h3>Profile</h3>
                <div><strong>Username:</strong> <span id="profileUsername"></span></div>
                <div><strong>Full Name:</strong> <span id="profileFullName"></span></div>
                <div><strong>Role:</strong> <span id="profileRole"></span></div>
            </div>

            <div class="card">
                <h3>Upcoming Sessions</h3>
                <table>
                    <thead>
                        <tr>
                            <th>Title</th>
                            <th>Cohort</th>
                            <th>Date</th>
                        </tr>
                    </thead>
                    <tbody id="sessionsTableBody"></tbody>
                </table>
            </div>
        </div>

        <div class="card" style="margin-top:16px;">
            <h3>Cohorts Attendance</h3>
            <table>
                <thead>
                    <tr>
                        <th>Cohort</th>
                        <th>Session Date</th>
                        <th>Present</th>
                        <th>Total</th>
                    </tr>
                </thead>
                <tbody id="attendanceTableBody"></tbody>
            </table>
        </div>
    </div>

    <script type="module" src="./js/trainer.js"></script>
</body>
</html>
```
