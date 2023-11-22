;==================================================================================================
; Item Receive & Removal
;==================================================================================================

.headersize G_CODE_DELTA

; Hook after item receive.
; Replaces:
;   jr      ra
.org 0x801143C4
    j       Items_AfterReceive_Hook

; Hook after item removal.
; Replaces:
;   jr      ra
.org 0x80114A94
    j       Items_AfterRemoval_Hook

;==================================================================================================
; Fix Item Receive for Custom Items
;==================================================================================================

.headersize G_CODE_DELTA

; Function begins at: 0x80112E80
; Prevent overwriting Ocarina inventory byte if item Id 0xA4 or higher.
; Replaces:
;   lui     t0, 0x801F
;   lbu     t8, 0x0047 (sp)
;   addiu   t0, t0, 0xF670
;   addu    t9, t0, t3
.org 0x801143A0
    jal     Items_ShouldTryWriteToInventory_Hook
    lui     t0, 0x801F
    beqzl   v0, 0x801143BC
    addiu   v0, r0, 0x00FF

;==================================================================================================
; Check if items usability should be checked while swimming
;==================================================================================================

.headersize G_CODE_DELTA

; Replaces:
;   ANDI    T7, A2, 0x00FF
;   ADDIU   AT, R0, 0x0034
;   BEQL    T7, AT, 0x801109B8
.org 0x80110890
    jal     Items_ShouldCheckItemUsabilityWhileSwimming_Hook
    lw      a0, 0x0080 (sp)
    beqzl   v0, 0x801109B8

;==================================================================================================
; Check environment hazard
;==================================================================================================

.headersize G_CODE_DELTA

; Replaces:
;   LW      T8, 0x0A6C (V0)
.org 0x80124318
    nop

; Replaces:
;   LW      T8, 0x0A6C (V0)
;   SLL     T9, T8, 4
;   BGEZ    T9, 0x8012437C
;   NOP
;   BNE     A0, V1, 0x80124374
;   NOP
;   LB      T0, 0x0145 (V0)
;   SLTI    AT, T0, 0x0005
;   BNEZ    AT, 0x80124374
;   NOP
;   LHU     T1, 0x0090 (V0)
;   ANDI    T2, T1, 0x0001
;   BEQZ    T2, 0x80124374
;   NOP
;   B       0x80124384
;   ADDIU   V1, R0, 0x0001
;   B       0x80124384
;   ADDIU   V1, R0, 0x0002
;   B       0x80124410
;   OR      V0, R0, R0
.org 0x80124334
.area 0x50, 0
    nop
    jal     Items_GetUnderwaterHazard
    or      a0, v0, r0
    b       0x80124410
    nop
.endarea
