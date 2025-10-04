"use client"

import { Autocomplete, AutocompleteItem } from "@heroui/react"
import React from "react"
import { MapPinAreaIcon } from "@phosphor-icons/react"
import { locals } from "@/data/local"

type LocalFilterProps = {
    value: string | null
    onChange: (value: string | null) => void
    className?: string
}
// cần truyền fetch data vào đang hard code
export function LocalFilter({ value, onChange, className }: LocalFilterProps) {
    return (
        <Autocomplete
            // defaultItems={locals}
            items={locals}
            label="Local"
            placeholder="Search"
            startContent={<MapPinAreaIcon className="text-xl" />}
            variant="bordered"
            // className="max-w-55 h-14 mr-0"
            className={className}
            selectedKey={value ?? undefined}
            onSelectionChange={(key) => {
                onChange(key as string)
                console.log("Selected:", key)
            }}
            // classNames={{
            //     base: "h-25", // toàn bộ khung
            //     selectorButton: "min-h-20 h-20 py-3", // ✅ phần hiển thị input
            //     listbox: "text-xl" // menu list
            // }}
        >
            {(item: (typeof locals)[0]) => (
                <AutocompleteItem key={item.key}>{item.label}</AutocompleteItem>
            )}
        </Autocomplete>
    )
}
