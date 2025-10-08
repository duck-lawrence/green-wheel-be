"use client"
import { useNavbarItemStore } from "@/hooks/singleton/store/useNavbarItemStore"
import React, { useEffect } from "react"
import { motion } from "framer-motion"
import { ButtonStyled } from "@/components"
import { Handshake, Leaf, Lightning } from "@phosphor-icons/react"
import { useTypewriter } from "@/utils/helpers/useTypewriter"

export default function AboutPage() {
    const setActiveMenuKey = useNavbarItemStore((s) => s.setActiveMenuKey)

    useEffect(() => {
        setActiveMenuKey("about")
    }, [setActiveMenuKey])

    const [title, titleDone] = useTypewriter("About Us", 50)
    const [desc] = useTypewriter(
        "Green Wheel — Vietnam’s leading electric car rental platform, delivering an eco-friendly, affordable, and seamless driving experience.",
        12,
        300
    )

    const visionContent = [
        {
            title: "Towards a Greener Future",
            desc: "We believe electric vehicles are the key to sustainable and environmentally friendly mobility.",
            icon: <Leaf size={48} weight="duotone" className="text-green-500" />
        },
        {
            title: "Customer-Centric Approach",
            desc: "Green Wheel prioritizes customer experience above all — ensuring convenience, transparency, and safety in every ride.",
            icon: <Handshake size={48} weight="duotone" className="text-blue-500" />
        },
        {
            title: "Continuous Innovation",
            desc: "We constantly improve our technology and expand our fleet with the latest electric vehicles to offer the best options available.",
            icon: <Lightning size={48} weight="duotone" className="text-yellow-500" />
        }
    ]

    return (
        <div>
            <section className="min-h-screen bg-white dark:bg-gray-950">
                {/* Hero section */}
                <div className="relative w-full h-[60vh] flex items-center justify-center overflow-hidden">
                    <img
                        src="https://baovephapluat.vn/data/images/0/2021/11/12/hienbt/8.jpg"
                        alt="Green Wheel electric cars"
                        fill
                        className="object-cover opacity-70 w-[70000px] object-cover"
                    />
                    <div className="absolute inset-0 bg-black/50 z-10" />
                    <motion.div
                        initial={{ opacity: 0, y: 40 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.6 }}
                        className="absolute z-20 text-center text-white max-w-2xl px-4"
                    >
                        <h1 className="text-4xl md:text-5xl font-bold mb-4 text-gray-200">
                            {title}
                            {!titleDone && <span className="animate-pulse">|</span>}
                        </h1>
                        <p className="text-lg text-gray-200 leading-relaxed">
                            {desc}
                            {desc.length > 0 && desc.length < 120 && (
                                <span className="animate-pulse">|</span>
                            )}
                        </p>
                    </motion.div>
                </div>

                {/* Introduction */}
                <div className="max-w-6xl mx-auto px-6 py-20">
                    <div className="grid md:grid-cols-2 gap-10 items-center">
                        <motion.div
                            initial={{ opacity: 0, x: -40 }}
                            whileInView={{ opacity: 1, x: 0 }}
                            transition={{ duration: 0.5 }}
                        >
                            <h2 className="text-3xl font-semibold text-gray-900 dark:text-white mb-6">
                                Our Mission
                            </h2>
                            <p className="text-gray-600 dark:text-gray-300 leading-relaxed mb-6">
                                Green Wheel was founded to accelerate the transition to clean,
                                sustainable transportation. We provide modern electric vehicles that
                                are environmentally friendly, helping customers enjoy smooth,
                                cost-effective, and eco-conscious journeys.
                            </p>
                            <ButtonStyled
                                color="primary"
                                variant="solid"
                                className="hover:text-black"
                            >
                                Learn more
                            </ButtonStyled>
                        </motion.div>

                        <motion.div
                            initial={{ opacity: 0, x: 40 }}
                            whileInView={{ opacity: 1, x: 0 }}
                            transition={{ duration: 0.5 }}
                            className="flex justify-center"
                        >
                            <img
                                src="https://sohanews.sohacdn.com/2020/4/14/777-15868600258581111429648.jpg"
                                alt="Eco friendly driving"
                                width={500}
                                height={350}
                                className="rounded-2xl shadow-lg object-cover"
                            />
                        </motion.div>
                    </div>
                </div>

                {/* Vision & Values */}
                <div className="bg-gray-50 dark:bg-gray-900 py-20">
                    <div className="max-w-6xl mx-auto px-6">
                        <h2 className="text-3xl font-semibold text-center mb-12 text-gray-900 dark:text-white">
                            Vision & Core Values
                        </h2>
                        <div className="grid md:grid-cols-3 gap-8">
                            {visionContent.map((item, key) => (
                                <motion.div
                                    key={key}
                                    initial={{ opacity: 0, y: 40 }}
                                    whileInView={{ opacity: 1, y: 0 }}
                                    transition={{
                                        delay: key * 0.1,
                                        duration: 0.5,
                                        ease: "easeOut"
                                    }}
                                    className="bg-white dark:bg-gray-800 rounded-2xl p-6 shadow-md hover:shadow-xl transition"
                                >
                                    <div className="text-4xl mb-4">{item.icon}</div>
                                    <h3 className="text-xl font-semibold mb-2 text-gray-900 dark:text-white">
                                        {item.title}
                                    </h3>
                                    <p className="text-gray-600 dark:text-gray-300">{item.desc}</p>
                                </motion.div>
                            ))}
                        </div>
                    </div>
                </div>

                {/* Contact CTA */}
                <div className="py-20 text-center">
                    <motion.h3
                        initial={{ opacity: 0, y: 40 }}
                        whileInView={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.5 }}
                        className="text-3xl font-bold mb-6 text-gray-900 dark:text-white"
                    >
                        Ready to join us on the road to a greener future?
                    </motion.h3>
                    <ButtonStyled
                        color="primary"
                        size="lg"
                        className="font-semibold hover:text-black"
                        onPress={() => (window.location.href = "/contact")}
                    >
                        Contact Us
                    </ButtonStyled>
                </div>
            </section>
        </div>
    )
}
