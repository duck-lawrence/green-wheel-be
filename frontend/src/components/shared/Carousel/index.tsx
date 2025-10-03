"use client"
import React, { useEffect, useMemo, useRef, useState } from "react"
import { ImageStyled } from "@/components/styled"

type CarouselProps = {
    slides: string[]
}

export const Carousel: React.FC<CarouselProps> = ({ slides }) => {
    const [currentIndex, setCurrentIndex] = useState(0)
    const [slidesPerView, setSlidesPerView] = useState(1)
    const [isTransitioning, setIsTransitioning] = useState(true)
    const startX = useRef<number | null>(null)

    // responsive
    useEffect(() => {
        const updateSlidesPerView = () => {
            if (window.innerWidth >= 1024) {
                setSlidesPerView(3)
            } else {
                setSlidesPerView(1)
            }
        }
        updateSlidesPerView()
        window.addEventListener("resize", updateSlidesPerView)
        return () => window.removeEventListener("resize", updateSlidesPerView)
    }, [])

    // clone đủ số lượng bằng slidesPerView
    const extendedSlides = useMemo(() => {
        const headClones = slides.slice(0, slidesPerView)
        const tailClones = slides.slice(-slidesPerView)
        return [...tailClones, ...slides, ...headClones]
    }, [slides, slidesPerView])

    // set index bắt đầu sau clone đầu
    useEffect(() => {
        setCurrentIndex(slidesPerView)
    }, [slidesPerView, slides])

    // Looping logic
    const nextSlide = () => {
        setCurrentIndex((prev) => prev + 1)
        setIsTransitioning(true)
    }

    const prevSlide = () => {
        setCurrentIndex((prev) => prev - 1)
        setIsTransitioning(true)
    }

    const goToSlide = (index: number) => {
        const targetIndex = index + slidesPerView

        if (targetIndex === currentIndex) return

        // Nếu đang ở cuối và click về đầu -> đi qua clone head
        if (currentIndex >= slides.length + slidesPerView - 1 && index === 0) {
            setIsTransitioning(true)
            setCurrentIndex(slides.length + slidesPerView) // sang clone head
            return
        }

        // Nếu đang ở đầu và click cuối -> đi qua clone tail
        if (currentIndex === slidesPerView && index === slides.length - 1) {
            setIsTransitioning(true)
            setCurrentIndex(slidesPerView - 1) // sang clone tail
            return
        }

        // Bình thường
        setIsTransitioning(true)
        setCurrentIndex(targetIndex)
    }

    const handleTransitionEnd = () => {
        // sang clone cuối -> nhảy về thật đầu
        if (currentIndex >= slides.length + slidesPerView) {
            setIsTransitioning(false)
            setCurrentIndex(slidesPerView)
        }
        // sang clone đầu -> nhảy về thật cuối
        if (currentIndex < slidesPerView) {
            setIsTransitioning(false)
            setCurrentIndex(slides.length + slidesPerView - 1)
        }
    }

    // Drag/swipe
    const handleTouchStart = (e: React.TouchEvent) => {
        startX.current = e.touches[0].clientX
    }

    const handleTouchEnd = (e: React.TouchEvent) => {
        if (startX.current !== null) {
            const endX = e.changedTouches[0].clientX
            const diff = startX.current - endX
            if (diff > 50) nextSlide()
            if (diff < -50) prevSlide()
        }
        startX.current = null
    }

    // index trung tâm
    const getCenterIndex = () => {
        if (slidesPerView === 1) return currentIndex
        if (slidesPerView === 3) return (currentIndex + 1) % extendedSlides.length
        return currentIndex
    }

    const centerIndex = getCenterIndex()

    return (
        <div className="flex flex-col items-center justify-center mt-10">
            <div className="relative w-full">
                {/* Carousel */}
                <div
                    className="overflow-hidden relative h-80"
                    onTouchStart={handleTouchStart}
                    onTouchEnd={handleTouchEnd}
                >
                    <div
                        className={`flex h-full ${
                            isTransitioning ? "transition-transform duration-500 ease-in-out" : ""
                        }`}
                        style={{
                            transform: `translateX(-${currentIndex * (100 / slidesPerView)}%)`
                        }}
                        onTransitionEnd={handleTransitionEnd}
                    >
                        {extendedSlides.map((src, i) => {
                            const isCenter = i === centerIndex
                            return (
                                <div
                                    key={i}
                                    className={`flex items-center justify-center p-6 transition-transform duration-500 ${
                                        isCenter ? "scale-110 z-10" : "scale-90 opacity-70"
                                    }`}
                                    style={{ flex: `0 0 ${100 / slidesPerView}%` }}
                                >
                                    {/* <div className="relative bg-gray-200 rounded-xl shadow-lg w-full h-full flex items-center justify-center"> */}
                                    <div className="relative overflow-hidden bg-white rounded-xl shadow-lg w-fit h-fit">
                                        <ImageStyled src={src} alt="" />
                                    </div>
                                </div>
                            )
                        })}
                    </div>
                    {/* Overlay nút bấm prev/next */}
                    <button
                        onClick={prevSlide}
                        className="absolute left-0 top-0 bottom-0 w-1/3 cursor-pointer z-20 bg-transparent"
                    />
                    <button
                        onClick={nextSlide}
                        className="absolute right-0 top-0 bottom-0 w-1/3 cursor-pointer z-20 bg-transparent"
                    />
                </div>

                {/* Dots */}
                <div className="absolute -bottom-6 left-0 right-0 flex justify-center gap-3">
                    {slides.map((_, i) => (
                        <button
                            key={i}
                            onClick={() => goToSlide(i)}
                            className={`w-3 h-3 rounded-full transition-colors ${
                                currentIndex - slidesPerView === i ? "bg-primary" : "bg-gray-400"
                            }`}
                        />
                    ))}
                </div>
            </div>
        </div>
    )
}
