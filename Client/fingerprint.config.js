module.exports = {
    files: "wwwroot/index.html",
    from: /(app|Client).min.css\?v=([0-9]*)/g,
    to: "$1.min.css?v=" + require("dayjs")().format("YYYYMMDDHHmm")
};