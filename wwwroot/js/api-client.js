const form = document.getElementById("request-form");
const operation = document.getElementById("operation");
const recordId = document.getElementById("record-id");
const requestBody = document.getElementById("request-body");
const sendButton = document.getElementById("send-button");
const error = document.getElementById("error");
const methods = { list: "GET", get: "GET", create: "POST", update: "PUT", delete: "DELETE" };

function updateFields() {
    recordId.disabled = operation.value === "list" || operation.value === "create";
    requestBody.disabled = operation.value !== "create" && operation.value !== "update";
}

operation.addEventListener("change", updateFields);
updateFields();

form.addEventListener("submit", async event => {
    event.preventDefault();
    error.textContent = "";
    const method = methods[operation.value];
    let url = `/api/${form.dataset.resource}`;
    if (!recordId.disabled) {
        const id = Number(recordId.value);
        if (!Number.isSafeInteger(id) || id < 1 || id > 2147483647) {
            error.textContent = "Введите целый ID от 1 до 2147483647";
            return;
        }
        url += `/${id}`;
    }

    const options = { method };
    if (!requestBody.disabled) {
        try {
            options.body = JSON.stringify(JSON.parse(requestBody.value));
            options.headers = { "Content-Type": "application/json" };
        } catch {
            error.textContent = "Проверьте формат JSON в теле запроса";
            return;
        }
    }

    sendButton.disabled = true;
    document.getElementById("response-method").textContent = method;
    document.getElementById("response-url").textContent = url;
    document.getElementById("response-status").textContent = "Ожидание ответа";
    document.getElementById("response-type").textContent = "Ожидание ответа";
    document.getElementById("response-body").textContent = "";
    try {
        const response = await fetch(url, options);
        const body = await response.text();
        document.getElementById("response-status").textContent = `${response.status} ${response.statusText}`.trim();
        document.getElementById("response-type").textContent = response.headers.get("Content-Type") || "Заголовок отсутствует";
        let formatted = body || "Ответ без тела";
        if (body) {
            try {
                formatted = JSON.stringify(JSON.parse(body), null, 2);
            } catch {
                formatted = body;
            }
        }
        document.getElementById("response-body").textContent = formatted;
    } catch {
        error.textContent = "Не удалось отправить запрос, проверьте доступность сервера";
        document.getElementById("response-status").textContent = "Ответ не получен";
        document.getElementById("response-type").textContent = "Ответ не получен";
    } finally {
        sendButton.disabled = false;
    }
});
