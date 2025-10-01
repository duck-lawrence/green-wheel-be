"use client"
import { OrderStatus } from "@/constants/enum"
import { OrderStatusLabels } from "@/constants/labels"
import { Autocomplete, AutocompleteItem } from "@heroui/react"
import React from "react"

type StatusOrderProps = {
    value: object | null
    onChange: (value: OrderStatus) => void
}
const statusItems = Object.values(OrderStatus)
    .filter((val) => typeof val === "number")
    .map((val) => ({
        key: String(val), // change key => string
        label: OrderStatusLabels[val as OrderStatus]
    }))

export function StatusOrderPicker({ value, onChange }: StatusOrderProps) {
    return (
        <Autocomplete
            items={statusItems}
            variant="bordered"
            isClearable
            className="max-w-55 h-14"
            label="Status"
            selectedKey={value !== null ? String(value) : undefined} //ép sang string mới hiện value trong ô
            onSelectionChange={(key) => {
                if (key !== null) {
                    onChange(Number(key) as OrderStatus) // convert ngược lại về number để lưu vào formik
                    console.log("Selected:", key)
                }
            }}
        >
            {(item) => <AutocompleteItem key={item.key}>{item.label}</AutocompleteItem>}
        </Autocomplete>
    )
}
