"use client"
import * as React from "react"
import { Textarea, type TextAreaProps  , cn } from "@heroui/react"

export const TextareaStyled = React.forwardRef<HTMLTextAreaElement, TextAreaProps>(
  ({ className, classNames, variant = "bordered", color = "primary", ...rest }, ref) => (
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
  )
)
TextareaStyled.displayName = "TextareaStyled"
