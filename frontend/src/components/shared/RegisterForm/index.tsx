"use client"
import { cn } from "@heroui/react"
import React, { useState } from "react"
import { AnimatePresence, motion } from "framer-motion"
import { RowStep } from "./RowStep"
import { RegisterEmail } from "./RegisterEmail"
import { RegisterOTP } from "./RegisterOTP"
import { RegisterInFo } from "./RegisterInFo"

export function RegisterForm({ onSuccess }: { onSuccess?: () => void }) {
    const [step, setStep] = useState(0)
    const [direction, setDirection] = useState(1) // 1 = next, -1 = back
    const [email, setEmail] = useState("")

    const variants = {
        enter: (direction: number) => ({
            x: direction > 0 ? 100 : -100,
            opacity: 0
        }),
        center: {
            x: 0,
            opacity: 1
        },
        exit: (direction: number) => ({
            x: direction > 0 ? -100 : 100,
            opacity: 0
        })
    }

    const handleNextStep = () => {
        setDirection(1)
        setStep((prev) => prev + 1)
    }

    const handlePrevStep = () => {
        setDirection(-1)
        setStep((prev) => prev - 1)
    }

    return (
        <div className="flex w-full h-full justify-center items-center">
            <div
                className={cn(
                    "flex flex-col gap-2 rounded-2xl transition-all duration-300 overflow-hidden",
                    step == 0 && "w-125 h-131",
                    step == 1 && "w-125 h-131",
                    step == 2 && "w-125 h-160"
                )}
            >
                <div className="ml-10 mt-6 overflow-hidden">
                    <RowStep n={step} />
                </div>

                <div className="relative flex-1">
                    <AnimatePresence mode="wait" custom={direction}>
                        {step === 0 && (
                            <motion.div
                                key="email"
                                custom={direction}
                                variants={variants}
                                initial="enter"
                                animate="center"
                                exit="exit"
                                transition={{ duration: 0.4 }}
                                className="absolute w-full"
                            >
                                <RegisterEmail
                                    email={email}
                                    setEmail={setEmail}
                                    onSuccess={handleNextStep}
                                />
                            </motion.div>
                        )}
                        {step === 1 && (
                            <motion.div
                                key="otp"
                                custom={direction}
                                variants={variants}
                                initial="enter"
                                animate="center"
                                exit="exit"
                                transition={{ duration: 0.4 }}
                                className="absolute w-full"
                            >
                                <RegisterOTP
                                    email={email}
                                    onBack={handlePrevStep}
                                    onSuccess={handleNextStep}
                                />
                            </motion.div>
                        )}
                        {step === 2 && (
                            <motion.div
                                key="info"
                                custom={direction}
                                variants={variants}
                                initial="enter"
                                animate="center"
                                exit="exit"
                                transition={{ duration: 0.4 }}
                                className="absolute w-full"
                            >
                                <RegisterInFo onSuccess={onSuccess} />
                            </motion.div>
                        )}
                    </AnimatePresence>
                </div>
            </div>
        </div>
    )
}
