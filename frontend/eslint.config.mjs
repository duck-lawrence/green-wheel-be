import parser from "@typescript-eslint/parser"
import pluginNext from "@next/eslint-plugin-next"
import { defineConfig } from "eslint/config"

export default defineConfig([
    {
        ignores: ["node_modules/**", ".next/**", "out/**", "build/**", "next-env.d.ts"]
    },
    {
        files: ["**/*.{js,mjs,cjs,ts,mts,cts,jsx,tsx}"],
        languageOptions: {
            parser, // optional
            parserOptions: {
                ecmaVersion: "latest",
                sourceType: "module",
                ecmaFeatures: {
                    jsx: true
                }
            }
        },
        plugins: { "@next/next": pluginNext },
        settings: {
            react: {
                version: "detect"
            }
        }
    },
    {
        rules: {
            ...pluginNext.configs.recommended.rules,
            ...pluginNext.configs["core-web-vitals"].rules,
            "react/display-name": "off",
            indent: ["error", 4],
            "linebreak-style": "off",
            quotes: ["error", "double"],
            semi: ["error", "never"]
        }
    }
])
