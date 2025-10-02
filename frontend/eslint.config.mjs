import js from "@eslint/js"
import pluginNext from "@next/eslint-plugin-next"
import tsEslint from "@typescript-eslint/eslint-plugin"
import tsParser from "@typescript-eslint/parser"
import reactHooks from "eslint-plugin-react-hooks"
import globals from "globals"

export default [
    js.configs.recommended,
    {
        files: ["**/*.{js,jsx,ts,tsx}"],
        languageOptions: {
            parser: tsParser,
            parserOptions: {
                ecmaVersion: "latest",
                sourceType: "module"
            },
            globals: {
                ...globals.browser,
                ...globals.node
            }
        },
        plugins: {
            "@next/next": pluginNext,
            "@typescript-eslint": tsEslint,
            "react-hooks": reactHooks
        },
        rules: {
            ...pluginNext.configs.recommended.rules,
            ...pluginNext.configs["core-web-vitals"].rules,
            "no-unused-vars": "off",
            "@typescript-eslint/no-unused-vars": "error",
            "react/display-name": "off",
            "react-hooks/rules-of-hooks": "error",
            "react-hooks/exhaustive-deps": "warn",
            indent: ["error", 4],
            "linebreak-style": "off",
            quotes: ["error", "double"],
            semi: ["error", "never"]
        }
    }
]

// import pluginNext from "@next/eslint-plugin-next"
// import parser from "@typescript-eslint/parser"
// import { defineConfig } from "eslint/config"

// export default defineConfig({
//     ignores: ["node_modules/**", ".next/**", "out/**", "build/**", "next-env.d.ts"],
//     languageOptions: {
//         parser,
//         parserOptions: {
//             ecmaVersion: "latest",
//             sourceType: "module",
//             ecmaFeatures: {
//                 jsx: true
//             }
//         }
//     },
//     plugins: {
//         "@next/next": pluginNext
//     },
//     settings: {
//         react: { version: "detect" }
//     },
//     files: ["**/*.ts", "**/*.tsx"],
//     rules: {
//         ...pluginNext.configs.recommended.rules,
//         "react/display-name": "off",
//         indent: ["error", 4],
//         "linebreak-style": "off",
//         quotes: ["error", "double"],
//         semi: ["error", "never"]
//     }
// })
