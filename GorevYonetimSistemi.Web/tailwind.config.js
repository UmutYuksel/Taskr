/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',
  content: [
    './*.html',
    './wwwroot/**/*.html',
    './Pages/**/*.cshtml',
    './Views/**/*.cshtml',
    './GorevYonetimSistemi.Web/**/*.cshtml'
  ],
  theme: {
    container: {
      center: true,
    },
    screens: {
      sm: '640px',
      md: '768px',
      lg: '1024px',
      xl: '1024px',
      '2xl' : '1024px',
    },
    extend: {
      fontFamily: {
        gemunu: ["Gemunu Libre", 'serif'], 
        open: ["Open Sans", "serif"]
      },
      colors: {
        "darkBackground" : "#222831",
        "lightBackground" : "#DFE0E1",

        "darkContent-0" : "#E1E2E3",
        "darkContent-50" : "#C4C5C8",
        "darkContent-100" : "#A6A8AC",
        "darkContent-200" : "#898C91",
        "darkContent-300" : "#6B6F75",
        "darkContent-400" : "#4E525A",
        "darkContent-500" : "#31363F",
        "darkContent-600" : "#2A2E36",
        "darkContent-700" : "#23262D",
        "darkContent-800" : "#1C1E24",
        "darkContent-900" : "#15171B",
        "darkContent-1000" : "#0E0F12",

        "darkAccent-100" : "#C4DBDC",
        "darkAccent-200" : "#B0CFD0",
        "darkAccent-300" : "#9DC3C5",
        "darkAccent-400" : "#89B7B9",
        "darkAccent-500" : "#76ABAE",
        "darkAccent-600" : "#659295",
        "darkAccent-700" : "#547A7C",
        "darkAccent-800" : "#436163",
        "darkAccent-900" : "#32494A",

        "darkText" : "#EEEEEE",
        "lightText" : "#101010"

      },
      spacing: {
        "128": "32rem",
      }
    },
  },
  plugins: [],
}
