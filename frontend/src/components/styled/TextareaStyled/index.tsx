"use client"
import * as React from "react"
import { Textarea, type TextAreaProps, cn } from "@heroui/react"

export const TextareaStyled = React.forwardRef<
  React.ElementRef<typeof Textarea>,
  TextAreaProps
>(({ className, classNames, variant = "bordered", color = "default", ...rest }, ref) => (
  <Textarea
    ref={ref}
    variant={variant}
    color={color}
    {...rest}
    className={cn("w-full", className)}
    classNames={{
      inputWrapper: "min-h-[88px]",
      ...classNames,
    }}
  />
))

TextareaStyled.displayName = "TextareaStyled"
