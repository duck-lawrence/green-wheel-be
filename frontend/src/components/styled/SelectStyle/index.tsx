"use client"
import React from "react"
import { Select, SelectItem, SelectProps } from "@heroui/select"

type Option = { value: string; label: string }

type Props = Omit<SelectProps, "children"> & {
  options: Option[]
  value?: string
  onSimpleChange?: (val: string) => void
  errorMessage?: string
}

export function SelectStyled({
  options,
  value,
  onSimpleChange,
  errorMessage,
  ...rest
}: Props) {
  return (
    <Select
      {...rest}
      selectedKeys={value ? [value] : []}
      onSelectionChange={(keys) => {
        const val = keys instanceof Set ? [...keys][0] : (keys as any).currentKey
        onSimpleChange?.(val || "")
      }}
      errorMessage={errorMessage}
      isInvalid={!!errorMessage}
      variant="bordered"
      size="md"
      className="w-full"
    >
      {options.map((o) => (
        <SelectItem key={o.value}>{o.label}</SelectItem>
      ))}
    </Select>
  )
}