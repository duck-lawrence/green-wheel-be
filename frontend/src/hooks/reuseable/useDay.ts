import { DEFAULT_DATE_TIME_FORMAT, DEFAULT_TIMEZONE } from "@/constants/constants"
import dayjs from "dayjs"
import { DateValue, parseDateTime } from "@internationalized/date"

export const useDay = ({
    defaultFormat = DEFAULT_DATE_TIME_FORMAT
}: {
    defaultFormat?: string
}) => {
    const toCalenderDateTime = (
        dateTime: string | number | Date | dayjs.Dayjs | null | undefined
    ) => {
        if (!dateTime) return null
        const str = dayjs(dateTime).format(defaultFormat)
        return parseDateTime(str)
    }

    const formatDateTime = ({
        date,
        timeZone = DEFAULT_TIMEZONE
    }: {
        date: DateValue
        timeZone?: string
    }) => {
        if (!date) return ""
        const dateJs = dayjs(date.toDate(timeZone))
        return dateJs.format(defaultFormat)
    }

    const diffDaysCeil = ({
        start,
        end
    }: {
        start: string | Date | dayjs.Dayjs
        end: string | Date | dayjs.Dayjs
    }) => {
        if (!start || !end) return -1
        return Math.ceil(dayjs(end).diff(dayjs(start), "day", true))
    }

    return { toCalenderDateTime, formatDateTime, diffDaysCeil }
}
