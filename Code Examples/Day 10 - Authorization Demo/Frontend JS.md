# Frontend JS (Day 10)

This file contains the client-side JavaScript for handling JWT tokens and interacting with the protected API endpoints for the Authorization Demo application.

### `wwwroot/js/auth.js`
```javascript
// @ts-check

const tokenKey = "chubb-training-token";

/**
 * @typedef {Object.<string, unknown>} JwtPayload
 */

/**
 * @param {string} token
 * @returns {void}
 */
export function saveToken(token) {
    sessionStorage.setItem(tokenKey, token);
}

/**
 * @returns {string | null}
 */
export function getToken() {
    return sessionStorage.getItem(tokenKey);
}

/**
 * @returns {void}
 */
export function clearToken() {
    sessionStorage.removeItem(tokenKey);
}

/**
 * @param {string | null} token
 * @returns {JwtPayload | null}
 */
function parseJwt(token) {
    if (!token) {
        return null;
    }

    const parts = token.split(".");
    if (parts.length !== 3) {
        return null;
    }

    try {
        const base64Url = parts[1];
        const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
        const padded = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), "=");
        const json = atob(padded);

        return /** @type {JwtPayload} */ (JSON.parse(json));
    } catch {
        return null;
    }
}

/**
 * @param {JwtPayload | null} payload
 * @param {string[]} keys
 * @returns {string}
 */
function getClaim(payload, keys) {
    if (!payload) {
        return "";
    }

    for (const key of keys) {
        const value = payload[key];

        if (typeof value === "string" && value.trim().length > 0) {
            return value;
        }
    }

    return "";
}

/**
 * @returns {JwtPayload | null}
 */
function getValidPayload() {
    const payload = parseJwt(getToken());

    if (!payload) {
        return null;
    }

    const exp = payload["exp"];
    if (typeof exp === "number" && Date.now() >= exp * 1000) {
        clearToken();
        return null;
    }

    return payload;
}

/**
 * @returns {string}
 */
export function getRole() {
    const payload = getValidPayload();

    return getClaim(payload, [
        "role",
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ]);
}

/**
 * @returns {string}
 */
export function getUsername() {
    const payload = getValidPayload();

    return getClaim(payload, [
        "username",
        "unique_name",
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
    ]);
}

/**
 * @returns {string}
 */
export function getFullName() {
    const payload = getValidPayload();

    return getClaim(payload, [
        "fullName",
        "FullName"
    ]);
}

/**
 * @returns {void}
 */
export function redirectByRole() {
    const role = getRole();

    if (role === "Admin") {
        window.location.replace("./admin.html");
        return;
    }

    if (role === "Trainer") {
        window.location.replace("./trainer.html");
        return;
    }

    clearToken();
    window.location.replace("./login.html");
}

/**
 * @param {string} expectedRole
 * @returns {boolean}
 */
export function requireRole(expectedRole) {
    const payload = getValidPayload();

    if (!payload) {
        window.location.replace("./login.html");
        return false;
    }

    const role = getRole();

    if (role !== expectedRole) {
        clearToken();
        window.location.replace("./login.html");
        return false;
    }

    return true;
}

/**
 * @returns {void}
 */
export function logout() {
    clearToken();
    window.location.replace("./login.html");
}

/**
 * @param {Response} response
 * @returns {Promise<any>}
 */
async function readResponseData(response) {
    if (response.status === 204) {
        return null;
    }

    const contentType = response.headers.get("content-type") ?? "";

    if (contentType.includes("application/json")) {
        return await response.json();
    }

    return await response.text();
}

/**
 * @param {unknown} data
 * @returns {string}
 */
function extractMessage(data) {
    if (typeof data === "string" && data.trim().length > 0) {
        return data;
    }

    if (typeof data === "object" && data !== null) {
        const candidate = /** @type {{ message?: unknown }} */ (data);

        if (typeof candidate.message === "string" && candidate.message.trim().length > 0) {
            return candidate.message;
        }
    }

    return "Request failed.";
}

/**
 * @param {string} url
 * @param {RequestInit} [options]
 * @returns {Promise<any>}
 */
export async function api(url, options = {}) {
    const headers = new Headers(options.headers ?? undefined);
    const token = getToken();

    if (options.body && !headers.has("Content-Type")) {
        headers.set("Content-Type", "application/json");
    }

    if (token) {
        headers.set("Authorization", `Bearer ${token}`);
    }

    const response = await fetch(url, {
        ...options,
        headers
    });

    const data = await readResponseData(response);

    if (response.status === 401 || response.status === 403) {
        clearToken();
        window.location.replace("./login.html");
        throw new Error(extractMessage(data));
    }

    if (!response.ok) {
        throw new Error(extractMessage(data));
    }

    return data;
}
```

### `wwwroot/js/admin.js`
```javascript
// @ts-check

import { api, getFullName, getUsername, logout, requireRole } from "./auth.js";

/**
 * @typedef {Object} Cohort
 * @property {number} id
 * @property {string} name
 * @property {string} startDate
 */

/**
 * @typedef {Object} TrainerInfo
 * @property {number} id
 * @property {string} username
 * @property {string} fullName
 * @property {string} specialty
 */

/**
 * @typedef {Object} AdminDashboard
 * @property {string=} message
 * @property {number=} cohortCount
 * @property {number=} trainerCount
 */

/**
 * @template {HTMLElement} T
 * @param {string} id
 * @returns {T}
 */
function byId(id) {
    const element = document.getElementById(id);

    if (!element) {
        throw new Error(`Element with id '${id}' was not found.`);
    }

    return /** @type {T} */ (element);
}

/**
 * @param {string} id
 * @param {string} value
 * @returns {void}
 */
function setText(id, value) {
    byId(id).textContent = value;
}

/**
 * @param {unknown} error
 * @returns {string}
 */
function getErrorMessage(error) {
    return error instanceof Error ? error.message : "Unexpected error.";
}

/**
 * @param {string} value
 * @returns {string}
 */
function formatDate(value) {
    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? value : date.toLocaleDateString();
}

/**
 * @param {string} message
 * @returns {HTMLLIElement}
 */
function createMessageItem(message) {
    const item = document.createElement("li");
    item.textContent = message;
    return item;
}

/**
 * @returns {Promise<void>}
 */
async function loadDashboard() {
    const data = /** @type {AdminDashboard} */ (await api("./api/admin/dashboard"));

    setText("welcomeText", `Logged in as ${getFullName()} (${getUsername()})`);
    setText("cohortCount", String(data.cohortCount ?? 0));
    setText("trainerCount", String(data.trainerCount ?? 0));
    setText("dashboardMessage", data.message ?? "");
}

/**
 * @returns {Promise<void>}
 */
async function loadCohorts() {
    const cohorts = /** @type {Cohort[]} */ (await api("./api/admin/cohorts"));
    const list = /** @type {HTMLUListElement} */ (byId("cohortList"));

    list.replaceChildren();

    if (cohorts.length === 0) {
        list.appendChild(createMessageItem("No cohorts found."));
        return;
    }

    for (const cohort of cohorts) {
        const item = document.createElement("li");
        const row = document.createElement("div");
        const info = document.createElement("div");
        const title = document.createElement("strong");
        const details = document.createElement("span");
        const button = document.createElement("button");

        row.className = "row";
        details.className = "muted";
        button.className = "danger";
        button.type = "button";
        button.textContent = "Remove";

        title.textContent = cohort.name;
        details.textContent = `Start: ${formatDate(cohort.startDate)}`;

        info.appendChild(title);
        info.appendChild(document.createElement("br"));
        info.appendChild(details);

        button.addEventListener("click", async () => {
            await deleteCohort(cohort.id);
        });

        row.appendChild(info);
        row.appendChild(button);
        item.appendChild(row);
        list.appendChild(item);
    }
}

/**
 * @returns {Promise<void>}
 */
async function loadTrainers() {
    const trainers = /** @type {TrainerInfo[]} */ (await api("./api/admin/trainers"));
    const list = /** @type {HTMLUListElement} */ (byId("trainerList"));

    list.replaceChildren();

    if (trainers.length === 0) {
        list.appendChild(createMessageItem("No trainers found."));
        return;
    }

    for (const trainer of trainers) {
        const item = document.createElement("li");
        const row = document.createElement("div");
        const info = document.createElement("div");
        const title = document.createElement("strong");
        const details = document.createElement("span");
        const button = document.createElement("button");

        row.className = "row";
        details.className = "muted";
        button.className = "danger";
        button.type = "button";
        button.textContent = "Remove";

        title.textContent = trainer.fullName;
        details.textContent = `${trainer.username} | ${trainer.specialty || "No specialty"}`;

        info.appendChild(title);
        info.appendChild(document.createElement("br"));
        info.appendChild(details);

        button.addEventListener("click", async () => {
            await deleteTrainer(trainer.id);
        });

        row.appendChild(info);
        row.appendChild(button);
        item.appendChild(row);
        list.appendChild(item);
    }
}

/**
 * @param {number} id
 * @returns {Promise<void>}
 */
async function deleteCohort(id) {
    await api(`./api/admin/cohorts/${id}`, {
        method: "DELETE"
    });

    await loadDashboard();
    await loadCohorts();
}

/**
 * @param {number} id
 * @returns {Promise<void>}
 */
async function deleteTrainer(id) {
    await api(`./api/admin/trainers/${id}`, {
        method: "DELETE"
    });

    await loadDashboard();
    await loadTrainers();
}

/**
 * @returns {Promise<void>}
 */
async function init() {
    if (!requireRole("Admin")) {
        return;
    }

    const logoutButton = /** @type {HTMLButtonElement} */ (byId("logoutButton"));
    const cohortForm = /** @type {HTMLFormElement} */ (byId("cohortForm"));
    const trainerForm = /** @type {HTMLFormElement} */ (byId("trainerForm"));
    const cohortNameInput = /** @type {HTMLInputElement} */ (byId("cohortName"));
    const cohortStartDateInput = /** @type {HTMLInputElement} */ (byId("cohortStartDate"));
    const trainerUsernameInput = /** @type {HTMLInputElement} */ (byId("trainerUsername"));
    const trainerPasswordInput = /** @type {HTMLInputElement} */ (byId("trainerPassword"));
    const trainerFullNameInput = /** @type {HTMLInputElement} */ (byId("trainerFullName"));
    const trainerSpecialtyInput = /** @type {HTMLInputElement} */ (byId("trainerSpecialty"));
    const cohortError = /** @type {HTMLDivElement} */ (byId("cohortError"));
    const trainerError = /** @type {HTMLDivElement} */ (byId("trainerError"));

    logoutButton.addEventListener("click", () => {
        logout();
    });

    cohortForm.addEventListener("submit", async (event) => {
        event.preventDefault();
        cohortError.textContent = "";

        try {
            await api("./api/admin/cohorts", {
                method: "POST",
                body: JSON.stringify({
                    name: cohortNameInput.value.trim(),
                    startDate: cohortStartDateInput.value
                })
            });

            cohortForm.reset();
            await loadDashboard();
            await loadCohorts();
        } catch (error) {
            cohortError.textContent = getErrorMessage(error);
        }
    });

    trainerForm.addEventListener("submit", async (event) => {
        event.preventDefault();
        trainerError.textContent = "";

        try {
            await api("./api/admin/trainers", {
                method: "POST",
                body: JSON.stringify({
                    username: trainerUsernameInput.value.trim(),
                    password: trainerPasswordInput.value,
                    fullName: trainerFullNameInput.value.trim(),
                    specialty: trainerSpecialtyInput.value.trim()
                })
            });

            trainerForm.reset();
            await loadDashboard();
            await loadTrainers();
        } catch (error) {
            trainerError.textContent = getErrorMessage(error);
        }
    });

    await Promise.all([
        loadDashboard(),
        loadCohorts(),
        loadTrainers()
    ]);
}

void init();
```

### `wwwroot/js/login.js`
```javascript
// @ts-check

import { getToken, redirectByRole, saveToken } from "./auth.js";

/**
 * @template {HTMLElement} T
 * @param {string} id
 * @returns {T}
 */
function byId(id) {
    const element = document.getElementById(id);

    if (!element) {
        throw new Error(`Element with id '${id}' was not found.`);
    }

    return /** @type {T} */ (element);
}

/**
 * @param {unknown} error
 * @returns {string}
 */
function getErrorMessage(error) {
    return error instanceof Error ? error.message : "Unexpected error.";
}

/**
 * @param {Response} response
 * @returns {Promise<any>}
 */
async function readResponseData(response) {
    const contentType = response.headers.get("content-type") ?? "";

    if (contentType.includes("application/json")) {
        return await response.json();
    }

    return await response.text();
}

/**
 * @param {unknown} data
 * @returns {string}
 */
function extractMessage(data) {
    if (typeof data === "string" && data.trim().length > 0) {
        return data;
    }

    if (typeof data === "object" && data !== null) {
        const candidate = /** @type {{ message?: unknown }} */ (data);

        if (typeof candidate.message === "string" && candidate.message.trim().length > 0) {
            return candidate.message;
        }
    }

    return "Login failed.";
}

/**
 * @param {unknown} data
 * @returns {string}
 */
function extractToken(data) {
    if (typeof data === "object" && data !== null) {
        const candidate = /** @type {{ token?: unknown, Token?: unknown }} */ (data);

        if (typeof candidate.token === "string" && candidate.token.length > 0) {
            return candidate.token;
        }

        if (typeof candidate.Token === "string" && candidate.Token.length > 0) {
            return candidate.Token;
        }
    }

    return "";
}

/**
 * @returns {void}
 */
function init() {
    if (getToken()) {
        redirectByRole();
        return;
    }

    const form = /** @type {HTMLFormElement} */ (byId("loginForm"));
    const usernameInput = /** @type {HTMLInputElement} */ (byId("username"));
    const passwordInput = /** @type {HTMLInputElement} */ (byId("password"));
    const errorMessage = /** @type {HTMLDivElement} */ (byId("errorMessage"));

    form.addEventListener("submit", async (event) => {
        event.preventDefault();
        errorMessage.textContent = "";

        try {
            const response = await fetch("./api/auth/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    username: usernameInput.value.trim(),
                    password: passwordInput.value
                })
            });

            const data = await readResponseData(response);

            if (!response.ok) {
                throw new Error(extractMessage(data));
            }

            const token = extractToken(data);

            if (!token) {
                throw new Error("Login response did not contain a token.");
            }

            saveToken(token);
            redirectByRole();
        } catch (error) {
            errorMessage.textContent = getErrorMessage(error);
        }
    });
}

init();
```

### `wwwroot/js/trainer.js`
```javascript
// @ts-check

import { api, getFullName, getUsername, logout, requireRole } from "./auth.js";

/**
 * @typedef {Object} TrainerDashboard
 * @property {string=} message
 * @property {number=} sessionCount
 * @property {number=} attendanceCount
 */

/**
 * @typedef {Object} Profile
 * @property {string=} username
 * @property {string=} fullName
 * @property {string=} role
 */

/**
 * @typedef {Object} TrainingSession
 * @property {number} id
 * @property {string} title
 * @property {string} cohortName
 * @property {string} sessionDate
 */

/**
 * @typedef {Object} AttendanceRecord
 * @property {number} id
 * @property {string} cohortName
 * @property {string} sessionDate
 * @property {number} presentCount
 * @property {number} totalCount
 */

/**
 * @template {HTMLElement} T
 * @param {string} id
 * @returns {T}
 */
function byId(id) {
    const element = document.getElementById(id);

    if (!element) {
        throw new Error(`Element with id '${id}' was not found.`);
    }

    return /** @type {T} */ (element);
}

/**
 * @param {string} id
 * @param {string} value
 * @returns {void}
 */
function setText(id, value) {
    byId(id).textContent = value;
}

/**
 * @param {string} value
 * @returns {string}
 */
function formatDateTime(value) {
    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? value : date.toLocaleString();
}

/**
 * @param {string[]} values
 * @returns {HTMLTableRowElement}
 */
function createRow(values) {
    const row = document.createElement("tr");

    for (const value of values) {
        const cell = document.createElement("td");
        cell.textContent = value;
        row.appendChild(cell);
    }

    return row;
}

/**
 * @param {number} columnCount
 * @param {string} message
 * @returns {HTMLTableRowElement}
 */
function createEmptyRow(columnCount, message) {
    const row = document.createElement("tr");
    const cell = document.createElement("td");

    cell.colSpan = columnCount;
    cell.textContent = message;

    row.appendChild(cell);

    return row;
}

/**
 * @returns {Promise<void>}
 */
async function loadDashboard() {
    const data = /** @type {TrainerDashboard} */ (await api("./api/trainer/dashboard"));

    setText("welcomeText", `Logged in as ${getFullName()} (${getUsername()})`);
    setText("sessionCount", String(data.sessionCount ?? 0));
    setText("attendanceCount", String(data.attendanceCount ?? 0));
    setText("dashboardMessage", data.message ?? "");
}

/**
 * @returns {Promise<void>}
 */
async function loadProfile() {
    const profile = /** @type {Profile} */ (await api("./api/secure/profile"));

    setText("profileUsername", profile.username ?? "");
    setText("profileFullName", profile.fullName ?? "");
    setText("profileRole", profile.role ?? "");
}

/**
 * @returns {Promise<void>}
 */
async function loadSessions() {
    const sessions = /** @type {TrainingSession[]} */ (await api("./api/trainer/sessions"));
    const body = /** @type {HTMLTableSectionElement} */ (byId("sessionsTableBody"));

    body.replaceChildren();

    if (sessions.length === 0) {
        body.appendChild(createEmptyRow(3, "No upcoming sessions."));
        return;
    }

    for (const session of sessions) {
        body.appendChild(createRow([
            session.title,
            session.cohortName,
            formatDateTime(session.sessionDate)
        ]));
    }
}

/**
 * @returns {Promise<void>}
 */
async function loadAttendance() {
    const attendance = /** @type {AttendanceRecord[]} */ (await api("./api/trainer/attendance"));
    const body = /** @type {HTMLTableSectionElement} */ (byId("attendanceTableBody"));

    body.replaceChildren();

    if (attendance.length === 0) {
        body.appendChild(createEmptyRow(4, "No attendance records."));
        return;
    }

    for (const item of attendance) {
        body.appendChild(createRow([
            item.cohortName,
            formatDateTime(item.sessionDate),
            String(item.presentCount),
            String(item.totalCount)
        ]));
    }
}

/**
 * @returns {Promise<void>}
 */
async function init() {
    if (!requireRole("Trainer")) {
        return;
    }

    const logoutButton = /** @type {HTMLButtonElement} */ (byId("logoutButton"));

    logoutButton.addEventListener("click", () => {
        logout();
    });

    await Promise.all([
        loadDashboard(),
        loadProfile(),
        loadSessions(),
        loadAttendance()
    ]);
}

void init();
```
