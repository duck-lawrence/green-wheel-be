"use client"

import { cn, DatePicker, DatePickerProps } from "@heroui/react"
import React from "react"

export function DateTimeStyled(props: DatePickerProps) {
    return (
        <div className="w-full max-w-xl flex flex-row gap-4">
            <DatePicker
                hideTimeZone
                hourCycle={24}
                granularity="minute"
                variant="bordered"
                {...props}
                className={cn("font-medium text-base", props.className)}
            />
        </div>
    )
}
