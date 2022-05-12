module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      screens: {
        'desktop': '960px',
        'tablet': '736px'
      }
    },
  },
  plugins: [],
}
