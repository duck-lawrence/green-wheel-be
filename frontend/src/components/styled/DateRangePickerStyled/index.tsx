"use client"
import { cn, DateRangePicker, DateRangePickerProps } from "@heroui/react"
import React from "react"
export function DateRangePickerStyled(props: DateRangePickerProps) {
    return (
        <DateRangePicker
            variant="bordered"
            color="primary"
            label="Day start to end"
            {...props}
            className={cn("w-[240] font-medium text-base", props.className)}
        />
    )
}
