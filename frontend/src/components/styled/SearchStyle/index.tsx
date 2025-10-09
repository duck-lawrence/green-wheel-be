"use client"

/**
 * Usage Guide
 * // Backend example
 * // GET /resource?keyword={value}
 *
 * // Frontend setup
 * // const [keyword, setKeyword] = useState("")
 * // import { SearchStyle } from "@/components"
 * // <SearchStyle
 * //   value={keyword}
 * //   onValueChange={setKeyword}
 * //   placeholder="Search anything..."
 * // />
 */

import React from "react"
import { Input, cn } from "@heroui/react"
import type { InputProps } from "@heroui/react"
import { MagnifyingGlass } from "@phosphor-icons/react"

type SlotOverrides = NonNullable<InputProps["classNames"]>

const DEFAULT_CLASSNAMES: SlotOverrides = {
    base: "",
    label: "",
    input: "text-sm",
    innerWrapper: "",
    mainWrapper: "",
    helperWrapper: "",
    inputWrapper:
        "h-12 px-5 border border-slate-200 bg-white text-slate-800 transition-colors data-[hover=true]:border-slate-300 data-[focus=true]:border-primary data-[focus-visible=true]:border-primary",
    clearButton: "",
    description: "",
    errorMessage: "",
    // startContent: "",
    // endContent: ""
}

const mergeClassNames = (overrides?: SlotOverrides): SlotOverrides => {
    if (!overrides) return DEFAULT_CLASSNAMES
    const result: Partial<SlotOverrides> = {}
    for (const key of Object.keys(DEFAULT_CLASSNAMES) as Array<keyof SlotOverrides>) {
        const base = DEFAULT_CLASSNAMES[key]
        const extra = overrides?.[key]
        result[key] = extra ? cn(base, extra) : base
    }
    return result as SlotOverrides
}

export type SearchStyleProps = Omit<InputProps, "variant" | "size" | "radius"> & {
    classNames?: SlotOverrides
    showIcon?: boolean
}

export function SearchStyle({ classNames, showIcon = true, startContent, ...props }: SearchStyleProps) {
    const mergedClassNames = mergeClassNames(classNames)
    const defaultIcon = (
        <MagnifyingGlass
            className="h-5 w-5 text-slate-400 transition-colors group-data-[focus=true]:text-primary group-data-[focus-visible=true]:text-primary"
            weight="bold"
            aria-hidden
            data-slot="search-icon"
        />
    )
    return (
        <Input
            {...props}
            variant="bordered"
            size="md"
            radius="lg"
            startContent={startContent ?? (showIcon ? defaultIcon : undefined)}
            classNames={mergedClassNames}
        />
    )
}
