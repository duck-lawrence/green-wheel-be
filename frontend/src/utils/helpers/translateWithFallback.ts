import { TFunction } from "i18next"

export const translateWithFallback = (
    t: TFunction<"common", undefined>,
    key: string,
    fallbackKey: string = "common.unexpected_error"
) => {
    const translation = (t as (key: string) => string)(key)
    if (translation === key) {
        return (t as (key: string) => string)(fallbackKey)
    }
    return translation
}
