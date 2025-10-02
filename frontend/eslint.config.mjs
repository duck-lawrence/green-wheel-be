import pluginNext from "@next/eslint-plugin-next"
import parser from "@typescript-eslint/parser"
import { defineConfig } from "eslint/config"

export default defineConfig({
    ignores: ["node_modules/**", ".next/**", "out/**", "build/**", "next-env.d.ts"],
    languageOptions: {
        parser,
        parserOptions: {
            ecmaVersion: "latest",
            sourceType: "module",
            ecmaFeatures: {
                jsx: true
            }
        }
    },
    plugins: {
        "@next/next": pluginNext
    },
    settings: {
        react: { version: "detect" }
    },
    files: ["**/*.ts", "**/*.tsx"],
    rules: {
        ...pluginNext.configs.recommended.rules,
        "react/display-name": "off",
        indent: ["error", 4],
        "linebreak-style": "off",
        quotes: ["error", "double"],
        semi: ["error", "never"]
    }
})
