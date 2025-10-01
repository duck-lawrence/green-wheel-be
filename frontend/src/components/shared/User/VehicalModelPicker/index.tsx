"use client"
import { Autocomplete, AutocompleteItem } from "@heroui/react"
import React from "react"

interface BrandItem {
    key: string
    label: string
}

export const brands: BrandItem[] = [
    { key: "1", label: "VF3" },
    { key: "2", label: "VF5" },
    { key: "3", label: "VF5 Plus" },
    { key: "4", label: "VF7" },
    { key: "5", label: "VF7S" },
    { key: "9", label: "VF9" }
]

type BrandFillterProps = {
    value: string | null
    onChange: (value: string) => void
}

export function VehicalModelPicker({ value, onChange }: BrandFillterProps) {
    return (
        // <div className="flex items-center">
        <Autocomplete
            items={brands}
            variant="bordered"
            isClearable
            // startContent={<MapPinAreaIcon className="text-xl" />}
            className="max-w-55 h-14"
            label="Branch"
            // placeholder="All"
            selectedKey={value ?? undefined}
            onSelectionChange={(key) => {
                onChange(key as string)
                console.log("Selected:", key)
            }}
        >
            {(item: (typeof brands)[0]) => (
                <AutocompleteItem key={item.key}>{item.label}</AutocompleteItem>
            )}
        </Autocomplete>
    )
}
