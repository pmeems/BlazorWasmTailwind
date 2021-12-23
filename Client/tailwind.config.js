module.exports = {
    content: [
        "./**/*.html",
        "./**/*.razor"
    ],
    theme: {
        extend: {
            gridTemplateRows: {
                // Complex site-specific row configuration
                'layout': "auto 1fr auto"
            },
            scale: {
                '-1': "-1"
            }
        }
    },
    plugins: []
}
