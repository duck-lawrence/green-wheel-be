"use client"

/**
 * Usage Guide
 * // Backend example
 * // GET /resource?filter={value}
 *
 * // Frontend setup
 * // const [value, setValue] = useState(new Set(["all"]))
 * // import { FilterTypeStyle } from "@/components"
 * // <FilterTypeStyle selectedKeys={value} onSelectionChange={setValue}>
 * //   <SelectItem key="all">All</SelectItem>
 * // </FilterTypeStyle>
 */

import React from "react"
import { Select, cn } from "@heroui/react"
import type { SelectProps } from "@heroui/react"
import { Funnel } from "lucide-react"

type SlotOverrides<T> = NonNullable<SelectProps<T>["classNames"]>

const DEFAULT_CLASSNAMES: SlotOverrides<any> = {
    base: "",
    trigger:
        "group h-12 px-5 border border-slate-200 bg-white text-slate-800 transition-colors data-[hover=true]:border-slate-300 data-[focus=true]:border-primary data-[focus-visible=true]:border-primary data-[open=true]:border-primary",
    listboxWrapper: "",
    popoverContent: "",
    label: "",
    //labelWrapper: "",
    //topContent: "",
    //bottomContent: "",
    value: "text-slate-800",
    helperWrapper: "",
    errorMessage: "",
    description: "",
    selectorIcon:
        "text-slate-500 transition-colors group-data-[focus=true]:text-primary group-data-[open=true]:text-primary",
    innerWrapper: "",
    //startContent: "",
    endContent: ""
}

const mergeClassNames = <T,>(overrides?: SlotOverrides<T>): SlotOverrides<T> => {
    if (!overrides) return DEFAULT_CLASSNAMES
    const result: Partial<SlotOverrides<T>> = {}
    for (const key of Object.keys(DEFAULT_CLASSNAMES) as Array<keyof SlotOverrides<T>>) {
        const base = DEFAULT_CLASSNAMES[key]
        const extra = overrides?.[key]
        result[key] = extra ? cn(base, extra) : base
    }
    return result as SlotOverrides<T>
}

export type FilterTypeStyleProps<T = object> = Omit<
    SelectProps<T>,
    "variant" | "size" | "radius"
> & {
    classNames?: SlotOverrides<T>
    showIcon?: boolean
}

export function FilterTypeStyle<T = object>({
    classNames,
    showIcon = true,
    startContent,
    ...props
}: FilterTypeStyleProps<T>) {
    const mergedClassNames = mergeClassNames<T>(classNames)
    const defaultIcon = (
        <Funnel
            className="h-5 w-5 text-slate-400 transition-colors group-data-[focus=true]:text-primary group-data-[open=true]:text-primary"
            aria-hidden
            data-slot="filter-icon"
        />
    )

    return (
        <Select
            {...props}
            variant="bordered"
            size="md"
            radius="lg"
            startContent={startContent ?? (showIcon ? defaultIcon : undefined)}
            classNames={mergedClassNames}
        />
    )
}
