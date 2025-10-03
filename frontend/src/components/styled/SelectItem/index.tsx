"use client"
import React from "react"
import { Select, SelectProps } from "@heroui/select"

export function SelectStyled(props: SelectProps) {
  return <Select variant="bordered" size="md" {...props} />
}