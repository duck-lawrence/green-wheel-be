"use client"

import { Autocomplete, AutocompleteProps, cn } from "@heroui/react"
import React from "react"

// type AutocompleteStyleProps = {
//     value: string | null
//     onChange: (value: string | null) => void
//     className?: string
// }
// cần truyền fetch data vào đang hard code
// export function AutocompleteStyle({ value, onChange, className }: AutocompleteStyleProps) {

export function AutocompleteStyle(props: AutocompleteProps) {
    return (
        <Autocomplete
            // defaultItems={locals}
            variant="bordered"
            // className="max-w-55 h-14 mr-0"
            // selectedKey={value ?? undefined}
            // onSelectionChange={(key) => {
            //     onChange(key as string)
            //     console.log("Selected:", key)
            // }}
            {...props}
            className={cn(props.className)}
            // classNames={{
            //     base: "h-25", // toàn bộ khung
            //     selectorButton: "min-h-20 h-20 py-3", // ✅ phần hiển thị input
            //     listbox: "text-xl" // menu list
            // }}
        />
    )
}
