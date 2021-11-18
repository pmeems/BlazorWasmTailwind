module.exports = {
    purge: {
        enabled: true,
        content: [
            './**/*.html',
            './**/*.razor'
        ],
    }, darkMode: false,
    theme: {
        extend: {
            gridTemplateRows: {
                // Complex site-specific row configuration
                'layout': 'auto 1fr auto',
            },
            scale: {
                '-1': '-1'
            }
        }
    },
    variants: {
        extend: {},
    },
    plugins: [],
}
