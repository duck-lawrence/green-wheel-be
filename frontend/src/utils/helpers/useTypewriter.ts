"use client"
import { useState, useEffect } from "react"
export function useTypewriter(text: string, speed: number = 100, startDelay: number = 0) {
    const [displayText, setDisplayText] = useState("")
    const [isDone, setIsDone] = useState(false)

    useEffect(() => {
        let index = 0
        const timeout = setTimeout(() => {
            const interval = setInterval(() => {
                setDisplayText(text.slice(0, index + 1))
                index++
                if (index === text.length) {
                    clearInterval(interval)
                    setIsDone(true)
                }
            }, speed)
        }, startDelay)

        return () => {
            clearTimeout(timeout)
            setIsDone(false)
            setDisplayText("")
        }
    }, [text, speed, startDelay])

    return [displayText, isDone] as const
}
