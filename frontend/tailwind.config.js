import { heroui } from "@heroui/react"

/** @type {import('tailwindcss').Config} */
export default {
    content: [
        "./index.html",
        "./src/**/*.{js,ts,jsx,tsx}",
        "./node_modules/@heroui/theme/dist/**/*.{js,ts,jsx,tsx}"
    ],
    theme: {
        extend: {}
    },
    darkMode: "class",
    plugins: [
        heroui({
            themes: {
                light: {
                    colors: {
                        primary: {
                            50: "#effbf3",
                            400: "#00A63E",
                            DEFAULT: "#00A63E",
                            foreground: "#ffffff"
                        },
                        secondary: {
                            DEFAULT: "#f4f4f4",
                            foreground: "#000000"
                        }
                    }
                }
            }
        })
    ]
}
