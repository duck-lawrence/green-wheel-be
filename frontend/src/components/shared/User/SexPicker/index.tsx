"use client"
import { cn, Select, SelectItem, SelectProps } from "@heroui/react"
import React from "react"

export const animals = [
    { key: "nam", label: "Nam" },
    { key: "nu", label: "Nữ" },
    { key: "khac", label: "Khác" }
]

// Loại bỏ `children` vì mình render sẵn
type SexPickerProps = Omit<SelectProps, "children">

export function SexPicker(props: SexPickerProps) {
    return (
        <div className="flex items-center">
            <Select
                isClearable
                {...props}
                className={cn("max-w-xs font-medium text-base", props.className)}
            >
                {animals.map((animal) => (
                    <SelectItem key={animal.key}>{animal.label}</SelectItem>
                ))}
            </Select>
        </div>
    )
}
