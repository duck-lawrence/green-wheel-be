import { t } from "i18next"

export const getKeyWithFallback = (
    key: string,
    fallbackKey: string = "common.unexpected_error"
) => {
    const translation = (t as (key: string) => string)(key)
    if (translation === key) {
        return (t as (key: string) => string)(fallbackKey)
    }
    return translation
}
