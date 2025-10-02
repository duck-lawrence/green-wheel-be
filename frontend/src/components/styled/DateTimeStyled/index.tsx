"use client"

import { cn, DatePicker, DatePickerProps } from "@heroui/react"
import { CalendarDateTime } from "@internationalized/date"
import React from "react"

type DateTimeStyledProps = Omit<DatePickerProps, "value" | "onChange"> & {
    value?: CalendarDateTime
    onChange?: (value: CalendarDateTime) => void
}

export default function DateTimeStyled({ value, onChange, ...props }: DateTimeStyledProps) {
    return (
        <div className="w-full max-w-xl flex flex-row gap-4">
            <DatePicker
                hideTimeZone
                granularity="second"
                label="Event Date & Time"
                variant="bordered"
                value={value}
                onChange={(calendarDateTime) => {
                    if (calendarDateTime && onChange) {
                        onChange(calendarDateTime) //  giữ CalendarDateTime trong state
                    }
                }}
                {...props}
                className={cn("font-medium text-base", props.className)}
            />
        </div>
    )
}
